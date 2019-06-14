using System;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents a base class for HTTP errors that occur during application execution.
    /// </summary>
    public abstract class HttpExceptionBase : Exception
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public abstract HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionBase"></see> class with status code InternalServerError.
        /// </summary>
        protected HttpExceptionBase() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionBase"></see> class with status code InternalServerError and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        protected HttpExceptionBase(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpExceptionBase"></see> class with status code InternalServerError, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        protected HttpExceptionBase(string message, Exception innerException) : base(message, innerException) { }
    }
}
