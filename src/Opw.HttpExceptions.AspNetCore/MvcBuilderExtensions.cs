using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder and Microsoft.Extensions.DependencyInjection.IMvcBuilder interfaces.
    /// </summary>
    public static class MvcBuilderExtensions
    {
        /// <summary>
        /// Adds HttpExceptions services to the services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcCoreBuilder.</param>
        /// <param name="configureOptions">An action used to configure the provided options.</param>
        public static IMvcCoreBuilder AddHttpExceptions(this IMvcCoreBuilder builder, Action<HttpExceptionsOptions> configureOptions = null)
        {
            builder.Services.AddHttpExceptions(configureOptions);

            var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<HttpExceptionsOptions>>();
            if (!options.Value.SuppressInvalidModelStateResponseFactoryOverride)
                builder.ConfigureApiBehaviorOptions(ConfigureApiBehaviorOptions);

            return builder;
        }

        /// <summary>
        /// Adds HttpExceptions services to the services collection.
        /// </summary>
        /// <param name="builder">The Microsoft.Extensions.DependencyInjection.IMvcBuilder.</param>
        /// <param name="configureOptions">An action used to configure the provided options.</param>
        public static IMvcBuilder AddHttpExceptions(this IMvcBuilder builder, Action<HttpExceptionsOptions> configureOptions = null)
        {
            builder.Services.AddHttpExceptions(configureOptions);

            var options = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<HttpExceptionsOptions>>();
            if (!options.Value.SuppressInvalidModelStateResponseFactoryOverride)
                builder.ConfigureApiBehaviorOptions(ConfigureApiBehaviorOptions);

            return builder;
        }

        internal static IServiceCollection AddHttpExceptions(this IServiceCollection services, Action<HttpExceptionsOptions> configureOptions = null)
        {
            services.AddOptions();
            if (configureOptions != null)
                services.Configure(configureOptions);

            services.ConfigureOptions<HttpExceptionsOptionsSetup>();

            return services;
        }

        private static void ConfigureApiBehaviorOptions(ApiBehaviorOptions options)
        {
#if NETSTANDARD2_0
            options.SuppressMapClientErrors = true;
            options.SuppressModelStateInvalidFilter = false;
            options.SuppressUseValidationProblemDetailsForInvalidModelStateResponses = true;
            options.InvalidModelStateResponseFactory = (actionContext) => HandleInvalidModelStateResponse(actionContext);
#endif

#if NETCOREAPP3_0
            options.SuppressMapClientErrors = true;
            options.SuppressModelStateInvalidFilter = false;
            options.InvalidModelStateResponseFactory = (actionContext) => HandleInvalidModelStateResponse(actionContext);
#endif
        }

        private static IActionResult HandleInvalidModelStateResponse(ActionContext actionContext)
        {
            // Should we be throwing an exception here?
            throw new InvalidModelException(actionContext.ModelState.ToDictionary());
            // The other options is to map the exception here are return a ProblemDetailsResult
            //var httpExceptionsOptions = actionContext.HttpContext.RequestServices.GetRequiredService<IOptions<HttpExceptionsOptions>>();
            //httpExceptionsOptions.Value.TryMap(ex, actionContext.HttpContext, out var problemDetails);
            //return new ProblemDetailsResult(problemDetails);
        }
    }
}
