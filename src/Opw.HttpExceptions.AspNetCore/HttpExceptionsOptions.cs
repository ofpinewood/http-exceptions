using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions.AspNetCore.Mappers;
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
        /// Register of the ExceptionMappers that will be used during mapping.
        /// </summary>
        public IDictionary<Type, ExceptionMapperDescriptor> ExceptionMapperDescriptors { get; set; } = new Dictionary<Type, ExceptionMapperDescriptor>();

        /// <summary>
        /// Gets or sets the ExceptionMapper collection that will be used during mapping.
        /// </summary>
        public ICollection<IExceptionMapper> ExceptionMappers { get; set; } = new List<IExceptionMapper>();

        /// <summary>
        /// Initializes the HttpExceptionsOptions.
        /// </summary>
        public HttpExceptionsOptions() { }

        internal bool TryMap(Exception exception, HttpContext context, out ProblemDetails problemDetails)
        {
            foreach (var mapper in ExceptionMappers)
            {
                if (mapper.TryMap(exception, context, out problemDetails))
                    return true;
            }

            problemDetails = default;
            return false;
        }
    }
}
