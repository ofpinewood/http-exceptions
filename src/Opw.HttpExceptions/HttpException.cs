using System;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Represents HTTP errors that occur during application execution.
    /// </summary>
    public abstract class HttpException : Exception
    {
        /// <summary>
        /// HTTP status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class with status code InternalServerError.
        /// </summary>
        public HttpException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class with status code InternalServerError and a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public HttpException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class with status code InternalServerError, a specified error message
        /// and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public HttpException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class with a specified status code.
        /// </summary>
        /// <param name="statusCode">The HTTP status code that is relavant for the exception.</param>
        public HttpException(HttpStatusCode statusCode) : base()
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class with a specified status code and a specified error message.
        /// </summary>
        /// <param name="statusCode">The HTTP status code that is relavant for the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public HttpException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpException"></see> class with a specified status code, a specified error message and
        /// a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="statusCode">The HTTP status code that is relavant for the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public HttpException(HttpStatusCode statusCode, string message, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
