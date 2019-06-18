using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    /// <summary>
    /// Default mapper for mapping HTTP response errors to ProblemDetails.
    /// </summary>
    public class HttpResponseMapper : IHttpResponseMapper
    {
        /// <summary>
        /// HttpExceptions options.
        /// </summary>
        protected IOptions<HttpExceptionsOptions> Options { get; }

        /// <summary>
        /// The status code to map for.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Initializes the HttpResponseMapper.
        /// </summary>
        public HttpResponseMapper(IOptions<HttpExceptionsOptions> options)
        {
            Options = options;
        }

        /// <summary>
        /// Can the given status code be mapped for.
        /// </summary>
        /// <param name="status">The status code to map for.</param>
        public bool CanMap(int status)
        {
            return Status == int.MinValue || Status == status;
        }

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the HTTP response error.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>A ProblemDetails representation of the response error.</returns>
        public ProblemDetails Map(HttpResponse response)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Maps the HTTP response error to ProblemDetails.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <param name="problemDetails">A ProblemDetails representation of the HTTP response error.</param>
        public bool TryMap(HttpResponse response, out ProblemDetails problemDetails)
        {
            problemDetails = default;

            if (!CanMap(response.StatusCode))
                return false;

            try
            {
                problemDetails = Map(response);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}