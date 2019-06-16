using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// A <see cref="IConfigureOptions{TOptions}"/> implementation which will setup the HttpExceptionsOptions.
    /// </summary>
    public class HttpExceptionsOptionsSetup : IConfigureOptions<HttpExceptionsOptions>
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes the HttpExceptionsOptionsSetup.
        /// </summary>
        public HttpExceptionsOptionsSetup(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Invoked to configure a HttpExceptionsOptions instance.
        /// </summary>
        /// <param name="options">The options instance to configure.</param>
        public void Configure(HttpExceptionsOptions options)
        {
            if (options.IncludeExceptionDetails == null)
                options.IncludeExceptionDetails = IncludeExceptionDetails;

            if (options.IsExceptionResponse == null)
                options.IsExceptionResponse = IsExceptionResponse;

            if (options.ExceptionMapperDescriptors.Count() == 0)
                options.ExceptionMapper<Exception, ExceptionMapper<Exception>>();

            options.ExceptionMappers.Clear();
            foreach (var exceptionMapperDescriptor in options.ExceptionMapperDescriptors.Select(i => i.Value))
                options.ExceptionMappers.Add(CreatExceptionMapper(exceptionMapperDescriptor));
        }

        private IExceptionMapper CreatExceptionMapper(ExceptionMapperDescriptor exceptionMapperDescriptor)
        {
            return (IExceptionMapper)ActivatorUtilities
                .CreateInstance(_serviceProvider, exceptionMapperDescriptor.Type, exceptionMapperDescriptor.Arguments);
        }

        private static bool IncludeExceptionDetails(HttpContext context)
        {
            return context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
        }

        private static bool IsExceptionResponse(HttpContext context)
        {
            if (context.Response.StatusCode < 400 || context.Response.StatusCode >= 600)
                return false;

            if (context.Response.ContentLength.HasValue)
                return false;

            if (string.IsNullOrEmpty(context.Response.ContentType))
                return true;

            return false;
        }
    }
}
