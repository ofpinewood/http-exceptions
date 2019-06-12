using Microsoft.Extensions.DependencyInjection;
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

            //services.TryAddSingleton<ProblemDetailsMarkerService, ProblemDetailsMarkerService>();
            //services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ProblemDetailsOptions>, ProblemDetailsOptionsSetup>());

            return services;
        }
    }
}
