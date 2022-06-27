using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using Esprima.Ast;
using Jint;
using Jint.Native.Function;
using Jint.Runtime;
using MyApp.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyApp.Framework.Helpers.Script
{
    /// <summary>
    /// Jint engine.
    /// </summary>
    public class JintEngine : IEngine
    {
        private readonly Engine _engine;

        public JintEngine(TimeSpan timeout, long? memoryLimit = null)
        {
            _engine = new Engine(options => options
                .Strict()
                .LimitRecursion(Core.Parameters.Script.RecursionDepthLimit)
                .TimeoutInterval(timeout)
                .LimitMemory(memoryLimit ?? Core.Parameters.Script.MemoryLimit)
#if DEBUG
                .DebugMode()
#endif
            );
        }

        /// <inheritdoc />
        public object Evaluate(string code)
        {
            var o = _engine.Execute(code).GetCompletionValue().ToObject();

            // Make sure ExpandoObject is in a list. Jint returns it in an array by default.
            var result = ExpandoObjectArrayToList(o);
            return result;
        }

        /// <inheritdoc />
        public void Execute(string code) => _engine.Execute(code);

        /// <inheritdoc />
        public void ExecuteFile(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                var file = streamReader.ReadToEnd();
                _engine.Execute(file);
            }
        }

        /// <inheritdoc />
        public object GetGlobalValue(string variableName)
        {
            var v = _engine.GetValue(variableName);
            object o;

            // Catch self referencing loop exception and convert it to a MyApp exception.
            try
            {
                o = v.ToObject();
            }
            catch (RecursionDepthOverflowException)
            {
                throw new ScriptSelfReferenceObjectException(variableName, v.GetType());
            }

            // Make sure expando objects are in lists. Jint wraps them in an array by default.
            return ExpandoObjectArrayToList(o);
        }

        /// <inheritdoc />
        public bool HasFunctionVariable(string functionName, string variableName)
        {
            var value = _engine.GetValue(functionName);

            if (!(value is ScriptFunctionInstance scriptFunctionInstance))
            {
                return false;
            }

            return HasIdentifier(scriptFunctionInstance.FunctionDeclaration, variableName);
        }

        /// <inheritdoc />
        public bool HasGlobalValue(string variableName) => _engine.Global.HasProperty(variableName);

        /// <inheritdoc />
        public void SetGlobalFunction(string functionName, Delegate functionDelegate) => _engine.SetValue(functionName, functionDelegate);

        /// <inheritdoc />
        public void SetGlobalValue(string variableName, object value)
        {
            if (string.IsNullOrWhiteSpace(variableName))
            {
                throw new ArgumentNullException(nameof(variableName));
            }

            // Cannot call Engine.SetValue as it will load the variable as a CLR object.
            // Convert it to a JSON object and load it as string instead.
            // Use JavaScriptDateTimeConverter so CLR dates are converted to JavaScript date objects.
            var json = JsonConvert.SerializeObject(value, new JavaScriptDateTimeConverter());
            
            _engine.Execute($"var {variableName} = {json}");
        }

        /// <inheritdoc />
        public ExpandoObject UnwrapException(Exception exception)
        {
            if ((exception as JavaScriptException)?.Error == null)
            {
                return null;
            }

            var runtimeType = ToRuntimeType(((JavaScriptException)exception).Error.ToObject());

            if (runtimeType is ExpandoObject eo && ((IDictionary<string, object>)eo).ContainsKey("name"))
            {
                return eo;
            }

            return null;
        }

        /// <inheritdoc />
        public object ToEngineType(object runtimeType) => runtimeType;

        /// <inheritdoc />
        public object ToRuntimeType(object engineType) => ExpandoObjectArrayToList(engineType);

        /// <summary>
        /// Casts ExpandoObject arrays to ExpandoObject lists.
        /// </summary>
        /// <param name="inputObject">Input object to cast.</param>
        /// <returns>Input object with ExpandoObject lists.</returns>
        private static object ExpandoObjectArrayToList(object inputObject)
        {
            switch (inputObject)
            {
                case IDictionary<string, object> dictionary:
                {
                    // Loop through dictionary values and check if there's any nested ExpandoObject array.
                    var keys = dictionary.Keys.ToList();

                    foreach (var key in keys)
                    {
                        dictionary[key] = ExpandoObjectArrayToList(dictionary[key]);
                    }

                    // Convert the dictionary to an ExpandoObject.
                    if (!(inputObject is ExpandoObject))
                    {
                        inputObject = DictionaryToExpando(dictionary);
                    }

                    break;
                }
                case object[] array:
                    inputObject = array.Select(ExpandoObjectArrayToList).ToList();
                    break;
            }

            return inputObject;
        }

        /// <summary>
        /// Casts an Dictionary to ExpandoObject.
        /// </summary>
        /// <param name="dictionary">Dictionary to cast.</param>
        /// <returns>ExpandoObject.</returns>
        private static ExpandoObject DictionaryToExpando(IDictionary<string, object> dictionary)
        {
            switch (dictionary)
            {
                case null:
                    return null;
                case ExpandoObject o:
                    return o;
            }

            var expando = new ExpandoObject();
            var expandoDictionary = (IDictionary<string, object>)expando;

            foreach (var keyValuePair in dictionary)
            {
                switch (keyValuePair.Value)
                {
                    case IDictionary<string, object> value:
                        expandoDictionary.Add(keyValuePair.Key, DictionaryToExpando(value));
                        break;
                    case IEnumerable enumerable:
                        expandoDictionary.Add(keyValuePair.Key, DictionaryToExpando(enumerable));
                        break;
                    default:
                        expandoDictionary.Add(keyValuePair);
                        break;
                }
            }

            return expando;
        }

        /// <summary>
        /// Casts dictionaries in an enumerable to expando objects.
        /// </summary>
        /// <param name="enumerable">Enumerable to cast.</param>
        /// <returns>List of values with ExpandoObject.</returns>
        private static List<object> DictionaryToExpando(IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                return null;
            }

            var list = new List<object>();

            foreach (var i in enumerable)
            {
                switch (i)
                {
                    case IDictionary<string, object> d:
                        list.Add(DictionaryToExpando(d));
                        break;
                    case IEnumerable e:
                        list.Add(DictionaryToExpando(e));
                        break;
                    default:
                        list.Add(i);
                        break;
                }
            }

            return list;
        }

        /// <summary>
        /// Check if the node is the identifier we are looking for, or any of its child nodes has the identifier.
        /// </summary>
        /// <param name="node">Syntax node.</param>
        /// <param name="identifierName">Name of the identifier.</param>
        /// <returns>True if the node has the identifier.</returns>
        private static bool HasIdentifier(INode node, string identifierName) =>
            node.Type == Nodes.Identifier && node is Identifier identifier && identifier.Name == identifierName ||
            node.ChildNodes.Any(n => HasIdentifier(n, identifierName));

        private static string GetExceptionMessage(Exception ex)
        {
            switch (ex)
            {
                case JavaScriptException javaScriptException:
                    return $@"{ex.Message}
Line: {javaScriptException.LineNumber}
Column: {javaScriptException.Column}
Call stack: {javaScriptException.CallStack}
";
                case MyAppException myAppException:
                    return $@"{ex.Message}
Details: {myAppException.InnerException?.Message}
Stack trace: {myAppException.InnerException?.StackTrace}
";
                default:
                    return ex.Message;
            }
        }
    }
}