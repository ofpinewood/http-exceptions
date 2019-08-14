using System;
using System.Net;
using System.Runtime.Serialization;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP NotFound (404) errors that occur during application execution.
    /// </summary>
    [Serializable]
    public class NotFoundException : HttpExceptionBase
    {
        /// <summary>
        /// HTTP status code NotFound (404).
        /// </summary>
        public override HttpStatusCode StatusCode { get; protected set; } = HttpStatusCode.NotFound;

        /// <summary>
        /// Gets or sets a link to the help file associated with this exception.
        /// For HttpExeptions a link to status code information https://tools.ietf.org/html/rfc7231.
        /// </summary>
        /// <returns>The Uniform Resource Name (URN) or Uniform Resource Locator (URL).</returns>
        public override string HelpLink { get; set; } = ResponseStatusCodeLink.NotFound;

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

        /// <summary>
        /// Initializes a new instance of the exception class with serialized data.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info">info</paramref> parameter is null.</exception>
        /// <exception cref="SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0).</exception>
        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
