using System;

namespace MyApp.Core
{
    /// <summary>
    /// An exception that occurs when a script object has self referencing loop.
    /// </summary>
    public class ScriptSelfReferenceObjectException : MyAppException
    {
        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="property">Property name.</param>
        /// <param name="type">Property type.</param>
        public ScriptSelfReferenceObjectException(string property, Type type) :
            base($"Self referencing loop detected for property '{property}' with type '{type}'.")
        {
        }

        public ScriptSelfReferenceObjectException()
        {
        }

        public ScriptSelfReferenceObjectException(string message) : base(message)
        {
        }

        public ScriptSelfReferenceObjectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
