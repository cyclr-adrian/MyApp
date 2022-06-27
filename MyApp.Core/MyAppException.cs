using System;

namespace MyApp.Core
{
    /// <summary>
    /// Errors that occur during application execution.
    /// </summary>
    public class MyAppException : Exception
    {
        /// <summary>
        /// Full exception message.
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// Error data.
        /// </summary>
        public object ErrorData { get; }

        /// <summary>
        /// Initializes a new instance of the Exception class.
        /// </summary>
        public MyAppException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MyAppException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and data.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="errorData">The data object that caused the error.</param>
        public MyAppException(string message, object errorData)
            : base(message)
        {
            ErrorData = errorData;
        }

        /// <summary>
        /// Initializes a new instance of the Exception class with a specified error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public MyAppException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
