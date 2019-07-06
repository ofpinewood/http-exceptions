using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#endif

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

            ConfigureExceptionMappers(options);
            ConfigureHttpResponseMappers(options);
        }

        private static bool IncludeExceptionDetails(HttpContext context)
        {
#if NETSTANDARD2_0
            return context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
#endif
#if NETCOREAPP3_0
            return context.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName == Environments.Development;
#endif
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

        private void ConfigureExceptionMappers(HttpExceptionsOptions options)
        {
            if (options.ExceptionMapperDescriptors.Count() == 0)
                options.ExceptionMapper<Exception, ProblemDetailsExceptionMapper<Exception>>();

            options.ExceptionMappers.Clear();
            foreach (var exceptionMapperDescriptor in options.ExceptionMapperDescriptors.Select(i => i.Value))
                options.ExceptionMappers.Add(CreatExceptionMapper(exceptionMapperDescriptor));
        }

        private IExceptionMapper CreatExceptionMapper(ExceptionMapperDescriptor exceptionMapperDescriptor)
        {
            return (IExceptionMapper)ActivatorUtilities
                .CreateInstance(_serviceProvider, exceptionMapperDescriptor.Type, exceptionMapperDescriptor.Arguments);
        }

        private void ConfigureHttpResponseMappers(HttpExceptionsOptions options)
        {
            if (options.HttpResponseMapperDescriptors.Count() == 0)
                options.HttpResponseMapper<ProblemDetailsHttpResponseMapper>();

            options.HttpResponseMappers.Clear();
            foreach (var httpResponseMapperDescriptorItem in options.HttpResponseMapperDescriptors)
                options.HttpResponseMappers.Add(CreatHttpResponseMapper(httpResponseMapperDescriptorItem.Value, httpResponseMapperDescriptorItem.Key));
        }

        private IHttpResponseMapper CreatHttpResponseMapper(HttpResponseMapperDescriptor httpResponseMapperDescriptor, int status)
        {
            var httpResponseMapper = (IHttpResponseMapper)ActivatorUtilities
                .CreateInstance(_serviceProvider, httpResponseMapperDescriptor.Type, httpResponseMapperDescriptor.Arguments);
            httpResponseMapper.Status = status;
            return httpResponseMapper;
        }
    }
}
