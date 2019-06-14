using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for creating ProblemDetails from exceptions.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Creates and returns a ProblemDetails representation of the current exception.
        /// </summary>
        /// <param name="ex">The current exception.</param>
        /// <param name="includeExceptionDetails">Include ExceptionDetails or not.</param>
        /// <returns>A ProblemDetails representation of the current exception.</returns>
        public static ProblemDetails ToProblemDetails(this Exception ex, bool includeExceptionDetails = false)
        {
            var problemDetails = new ProblemDetails
            {
                Status = (int)GetStatusCode(ex),
                Type = GetType(ex),
                Title = GetName(ex),
                Detail = GetDetail(ex),
                Instance = GetHelpLink(ex)
            };

            if (includeExceptionDetails)
                problemDetails.Extensions.Add(nameof(ExceptionDetails).ToCamelCase(), new ExceptionDetails(ex));

            return problemDetails;
        }

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the current exception.
        /// </summary>
        /// <param name="ex">The current exception.</param>
        /// <param name="requestPath">A URI reference that identifies the specific occurrence of the problem.</param>
        /// <param name="includeExceptionDetails">Include ExceptionDetails or not.</param>
        /// <returns>A ProblemDetails representation of the current exception.</returns>
        public static ProblemDetails ToProblemDetails(this Exception ex, string requestPath, bool includeExceptionDetails = false)
        {
            var problemDetails = ex.ToProblemDetails(includeExceptionDetails);
            problemDetails.Instance = requestPath;

            return problemDetails;
        }

        internal static string GetHelpLink(Exception exception)
        {
            var link = exception.HelpLink;

            if (string.IsNullOrEmpty(link))
                return null;

            if (Uri.TryCreate(link, UriKind.Absolute, out var result))
                return result.ToString();

            return null;
        }

        internal static HttpStatusCode GetStatusCode(Exception ex)
        {
            if (ex is HttpExceptionBase httpException)
                return httpException.StatusCode;

            return HttpStatusCode.InternalServerError;
        }

        internal static string GetName(Exception ex)
        {
            var name = ex.GetType().Name;
            if (name.Contains("`")) name = name.Substring(0, name.IndexOf('`'));
            if (name.EndsWith("Exception", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring(0, name.Length - "Exception".Length);

            return name;
        }

        internal static string GetDetail(Exception ex)
        {
            return ex.Message;
        }

        internal static string GetType(Exception ex)
        {
            return new Uri($"error:{GetName(ex)}").ToString();
        }
    }
}
