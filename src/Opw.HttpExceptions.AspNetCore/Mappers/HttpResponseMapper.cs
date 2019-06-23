using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net;

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
        public int Status { get; set; } = int.MinValue;

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

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the HTTP response error.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>A ProblemDetails representation of the response error.</returns>
        public ProblemDetails Map(HttpResponse response)
        {
            if (!CanMap(response.StatusCode))
                throw new ArgumentOutOfRangeException(nameof(response), response, $"HttpResponse status is not {Status}.");

            var problemDetails = new ProblemDetails
            {
                Status = MapStatus(response),
                Type = MapType(response),
                Title = MapTitle(response),
                Detail = MapDetail(response),
                Instance = MapInstance(response)
            };

            return problemDetails;
        }

        /// <summary>
        /// Map the ProblemDetails.Instance property using the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>Returns the request path, or null.</returns>
        protected virtual string MapInstance(HttpResponse response)
        {
            if (response.HttpContext.Request?.Path.HasValue == true)
                return response.HttpContext.Request.Path;

            return null;
        }

        /// <summary>
        /// Map the ProblemDetails.Status property using the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>Returns the status of the response.</returns>
        protected virtual int MapStatus(HttpResponse response)
        {
            return response.StatusCode;
        }

        /// <summary>
        /// Map the ProblemDetails.Title property using the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>Returns the HTTP status name or the status code.</returns>
        protected virtual string MapTitle(HttpResponse response)
        {
            var status = response.StatusCode.ToString();
            try
            {
                status = ((HttpStatusCode)response.StatusCode).ToString();
            }
            catch { }

            return status;
        }

        /// <summary>
        /// Map the ProblemDetails.Detail property using the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>Returns the HTTP status name or the status code.</returns>
        protected virtual string MapDetail(HttpResponse response)
        {
            return MapTitle(response);
        }

        /// <summary>
        /// Map the ProblemDetails.Type property using the HTTP response.
        /// </summary>
        /// <param name="response">The HTTP response.</param>
        /// <returns>Returns the URI with the HTTP status name ("error:[status:slug]").</returns>
        protected virtual string MapType(HttpResponse response)
        {
            return new Uri($"error:{MapTitle(response).ToSlug()}").ToString();
        }
    }
}