using System;
using System.Dynamic;

namespace MyApp.Framework.Helpers.Script
{
    /// <summary>
    ///Script engine.
    /// </summary>
    public interface IEngine
    {
        /// <summary>
        /// Executes a statement and returns its result.
        /// </summary>
        /// <param name="code">Code to execute.</param>
        /// <returns>Script result.</returns>
        object Evaluate(string code);

        /// <summary>
        /// Executes a script.
        /// </summary>
        /// <param name="code">Code to execute.</param>
        void Execute(string code);

        /// <summary>
        /// Executes a script in the file.
        /// </summary>
        /// <param name="path">Path to the file containing the script.</param>
        void ExecuteFile(string path);

        /// <summary>
        /// Gets the global value.
        /// </summary>
        /// <param name="variableName">Name of the global value to get.</param>
        /// <returns>Global value.</returns>
        object GetGlobalValue(string variableName);

        /// <summary>
        /// Checks if a variable exists in a function.
        /// </summary>
        /// <param name="functionName">Name of the function to get.</param>
        /// <param name="variableName">Name of the variable to check.</param>
        /// <returns>True if the function has the variable.</returns>
        bool HasFunctionVariable(string functionName, string variableName);

        /// <summary>
        /// Checks if a global value exists.
        /// </summary>
        /// <param name="variableName">Name of the global value to check.</param>
        /// <returns>True if the global value exists.</returns>
        bool HasGlobalValue(string variableName);

        /// <summary>
        /// Sets a global function.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="functionDelegate">Function delegate.</param>
        void SetGlobalFunction(string functionName, Delegate functionDelegate);

        /// <summary>
        /// Sets a global variable.
        /// </summary>
        /// <param name="variableName">Name of the variable.</param>
        /// <param name="value">Value of the variable.</param>
        void SetGlobalValue(string variableName, object value);

        /// <summary>
        /// Unwraps engine exceptions.
        /// </summary>
        /// <param name="exception">Exception thrown from the engine.</param>
        /// <returns>Object that contains the exception cause.</returns>
        ExpandoObject UnwrapException(Exception exception);

        /// <summary>
        /// Converts a CLR value to a type accepted by the engine.
        /// </summary>
        /// <param name="runtimeType">CLR value.</param>
        /// <returns>Value in a type accepted by the engine.</returns>
        object ToEngineType(object runtimeType);

        /// <summary>
        /// Converts a value from the engine to its CLR type.
        /// </summary>
        /// <param name="engineType">Value from the engine.</param>
        /// <returns>CLR value.</returns>
        object ToRuntimeType(object engineType);
    }
}