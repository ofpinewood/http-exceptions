using System;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP Conflict (409) errors that occur during application execution.
    /// </summary>
    public class ConflictException : HttpExceptionBase
    {
        /// <summary>
        /// HTTP status code Conflict (409).
        /// </summary>
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.Conflict;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"></see> class with status code Conflict.
        /// </summary>
        public ConflictException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"></see> class with status code Conflict and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ConflictException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConflictException"></see> class with status code Conflict, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public ConflictException(string message, Exception innerException) : base(message, innerException) { }
    }
}
