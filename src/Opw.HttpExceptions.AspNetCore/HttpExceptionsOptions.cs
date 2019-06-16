using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Represents the possible HttpExceptions options for services.
    /// </summary>
    public class HttpExceptionsOptions
    {
        /// <summary>
        /// Include exception details, default behavior is only to include exception details in a development environment.
        /// </summary>
        public Func<HttpContext, bool> IncludeExceptionDetails { get; set; }

        /// <summary>
        /// Is the response an exception and should it be handled by the HttpExceptions middleware.
        /// </summary>
        public Func<HttpContext, bool> IsExceptionResponse { get; set; }

        /// <summary>
        /// Gets or sets a ExceptionMapper collection that will be used during mapping.
        /// </summary>
        public ICollection<ExceptionMapper<Exception>> ExceptionMappers { get; set; } = new List<ExceptionMapper<Exception>>();

        /// <summary>
        /// Initializes the HttpExceptionsOptions.
        /// </summary>
        public HttpExceptionsOptions() { }
    }
}
