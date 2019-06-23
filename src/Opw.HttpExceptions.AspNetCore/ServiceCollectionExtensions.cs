using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            services.AddOptions();
            if (configureOptions != null)
                services.Configure(configureOptions);

            services.ConfigureOptions<HttpExceptionsOptionsSetup>();

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<HttpExceptionsOptions>>();
            if (options.Value.UseInvalidModelStateResponseFactory)
                UseInvalidModelStateResponseFactory(services);

            return services;
        }

        private static IServiceCollection UseInvalidModelStateResponseFactory(IServiceCollection services)
        {
            services.AddMvcCore().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = true;
                options.SuppressModelStateInvalidFilter = false;
                options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = true;
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    // Should we be throwing an exception here?
                    throw new InvalidModelException(actionContext.ModelState.ToDictionary());
                    // The other options is to map the exception here are return a ProblemDetailsResult
                    //var httpExceptionsOptions = actionContext.HttpContext.RequestServices.GetRequiredService<IOptions<HttpExceptionsOptions>>();
                    //httpExceptionsOptions.Value.TryMap(ex, actionContext.HttpContext, out var problemDetails);
                    //return new ProblemDetailsResult(problemDetails);
                };
            });

            return services;
        }
    }
}
