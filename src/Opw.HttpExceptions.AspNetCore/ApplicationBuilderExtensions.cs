using Microsoft.AspNetCore.Builder;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the Microsoft.AspNetCore.Builder.IApplicationBuilder interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds middleware for HttpExceptions.
        /// </summary>
        /// <param name="app">The Microsoft.AspNetCore.Builder.IApplicationBuilder instance this method extends.</param>
        /// <returns>The Microsoft.AspNetCore.Builder.IApplicationBuilder,</returns>
        public static IApplicationBuilder UseHttpExceptions(this IApplicationBuilder app)
        {
            //var markerService = app.ApplicationServices.GetService<ProblemDetailsMarkerService>();

            //if (markerService is null)
            //{
            //    throw new InvalidOperationException(
            //        $"Please call {nameof(IServiceCollection)}.{nameof(AddProblemDetails)} in ConfigureServices before adding the middleware.");
            //}

            return app.UseMiddleware<HttpExceptionsMiddleware>();
        }
    }
}
