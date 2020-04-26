using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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

            if (options.ShouldLogException == null)
                options.ShouldLogException = ShouldLogException;

            if (options.ExceptionTypeMapping == null)
                options.ExceptionTypeMapping = ExceptionTypeMapping;

            if (options.HttpContextTypeMapping == null)
                options.HttpContextTypeMapping = HttpContextTypeMapping;

            ConfigureExceptionMappers(options);
            ConfigureHttpResponseMappers(options);
        }

        private static bool IncludeExceptionDetails(HttpContext context)
        {
            return context.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName == Environments.Development;
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

        private static bool ShouldLogException(Exception ex)
        {
            return true;
        }

        private static Uri ExceptionTypeMapping(Exception ex)
        {
            if (!string.IsNullOrWhiteSpace(ex.HelpLink))
            {
                try
                {
                    return new Uri(ex.HelpLink);
                }
                catch { }
            }
            return null;
        }

        private static Uri HttpContextTypeMapping(HttpContext context)
        {
            if (context.Response.StatusCode.TryGetInformationLink(out var url))
            {
                try
                {
                    return new Uri(url);
                }
                catch { }
            }
            return null;
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
