using System;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP NotFound (404) errors that occur during application execution.
    /// </summary>
    public class NotFoundException : HttpExceptionBase
    {
        /// <summary>
        /// HTTP status code NotFound (404).
        /// </summary>
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"></see> class with status code NotFound.
        /// </summary>
        public NotFoundException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"></see> class with status code NotFound and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public NotFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"></see> class with status code NotFound, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
