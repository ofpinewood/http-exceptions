using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// Maps the HTTP response error to ProblemDetails.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="problemDetails">A ProblemDetails representation of the HTTP response error.</param>
        bool TryMap(HttpResponse response, out ProblemDetails problemDetails);

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the HTTP response error.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>A ProblemDetails representation of the HTTP response error.</returns>
        ProblemDetails Map(HttpResponse response);
    }
}