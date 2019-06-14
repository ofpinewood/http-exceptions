using System;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP ServiceUnavailable (503) errors that occur during application execution.
    /// </summary>
    public class ServiceUnavailableException : HttpExceptionBase
    {
        /// <summary>
        /// HTTP status code ServiceUnavailable (503).
        /// </summary>
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.ServiceUnavailable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException"></see> class with status code ServiceUnavailable.
        /// </summary>
        public ServiceUnavailableException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException"></see> class with status code ServiceUnavailable and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ServiceUnavailableException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceUnavailableException"></see> class with status code ServiceUnavailable, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException) { }
    }
}
