using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    /// <summary>
    /// Interface for HttpResponseMappers.
    /// </summary>
    public interface IHttpResponseMapper
    {
        /// <summary>
        /// The status code to map for.
        /// </summary>
        int Status { get; set; }

        /// <summary>
        /// Can the given status code be mapped for.
        /// </summary>
        /// <param name="status">The status code to map for.</param>
        bool CanMap(int status);

        /// <summary>
        /// Maps the HTTP response error.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="actionResult">A representation of the HTTP response error as a IActionResult.</param>
        bool TryMap(HttpResponse response, out IStatusCodeActionResult actionResult);

        /// <summary>
        /// Creates and returns a representation of the HTTP response error as a IActionResult.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>A representation of the HTTP response error as a IActionResult.</returns>
        IStatusCodeActionResult Map(HttpResponse response);
    }
}