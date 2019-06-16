using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// A <see cref="IConfigureOptions{TOptions}"/> implementation which will setup the HttpExceptionsOptions.
    /// </summary>
    public class HttpExceptionsOptionsSetup : IConfigureOptions<HttpExceptionsOptions>
    {
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
        }

        private static bool IncludeExceptionDetails(HttpContext context)
        {
            return context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
        }

        private static bool IsExceptionResponse(HttpContext context)
        {
            if (context.Response.StatusCode < 400 && context.Response.StatusCode >= 600)
                return false;

            if (context.Response.ContentLength.HasValue)
                return false;

            if (string.IsNullOrEmpty(context.Response.ContentType))
                return true;

            return false;
        }
    }
}
