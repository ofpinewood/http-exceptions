using System;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP UnsupportedMediaType (415) errors that occur during application execution.
    /// </summary>
    public class UnsupportedMediaTypeException : HttpExceptionBase
    {
        /// <summary>
        /// HTTP status code UnsupportedMediaType (415).
        /// </summary>
        public override HttpStatusCode StatusCode { get; } = HttpStatusCode.UnsupportedMediaType;

        /// <summary>
        /// Gets or sets a link to the help file associated with this exception.
        /// For HttpExeptions a link to status code information https://tools.ietf.org/html/rfc7231.
        /// </summary>
        /// <returns>The Uniform Resource Name (URN) or Uniform Resource Locator (URL).</returns>
        public override string HelpLink { get; set; } = ResponseStatusCodeLink.UnsupportedMediaType;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"></see> class with status code UnsupportedMediaType.
        /// </summary>
        public UnsupportedMediaTypeException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"></see> class with status code UnsupportedMediaType and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public UnsupportedMediaTypeException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsupportedMediaTypeException"></see> class with status code UnsupportedMediaType, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public UnsupportedMediaTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
