using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds HttpExceptions services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddHttpExceptions(this IServiceCollection services)
        {
            return services.AddHttpExceptions(configureOptions: null);
        }

        /// <summary>
        /// Adds HttpExceptions services to the specified services collection.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configureOptions">An action used to configure the provided options.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddHttpExceptions(this IServiceCollection services, Action<HttpExceptionsOptions> configureOptions)
        {
            if (configureOptions != null)
                services.Configure(configureOptions);

            services.Configure((HttpExceptionsOptions options) => {
                // this is the default behavior; only include exception details in a development environment.
                options.IncludeExceptionDetails = context => context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();

                // This will map NotImplementedException to the 501 Not Implemented status code.
                //options.Map<NotImplementedException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status501NotImplemented));
                // This will map HttpRequestException to the 503 Service Unavailable status code.
                //options.Map<HttpRequestException>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status503ServiceUnavailable));
                // Because exceptions are handled polymorphically, this will act as a "catch all" mapping, which is why it's added last.
                // If an exception other than NotImplementedException and HttpRequestException is thrown, this will handle it.
                //options.Map<Exception>(ex => new ExceptionProblemDetails(ex, StatusCodes.Status500InternalServerError));
            });

            //services.TryAddSingleton<ProblemDetailsMarkerService, ProblemDetailsMarkerService>();
            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<HttpExceptionsOptions>, HttpExceptionsOptionsSetup>());

            return services;
        }
    }
}
