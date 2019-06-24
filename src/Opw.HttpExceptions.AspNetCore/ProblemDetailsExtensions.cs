using Microsoft.AspNetCore.Mvc;
#if NETSTANDARD2_0
using Newtonsoft.Json.Linq;
#endif

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for creating ProblemDetails from exceptions.
    /// </summary>
    public static class ProblemDetailsExtensions
    {
        /// <summary>
        /// Gets the ExceptionDetails of the current ProblemDetails.
        /// </summary>
        /// <param name="problemDetails">The current ProblemDetails.</param>
        /// <param name="exceptionDetails">The ExceptionDetails</param>
        /// <returns>The ExceptionDetails of the current ProblemDetails or null if there are none.</returns>
        public static bool TryGetExceptionDetails(this ProblemDetails problemDetails, out ExceptionDetails exceptionDetails)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ExceptionDetails).ToCamelCase(), out var value))
                return value.TryParseExceptionDetails(out exceptionDetails);

            exceptionDetails = null;
            return false;
        }

        internal static bool TryParseExceptionDetails(this object value, out ExceptionDetails exceptionDetails)
        {
            exceptionDetails = null;

            if (value is ExceptionDetails)
                exceptionDetails = (ExceptionDetails)value;
#if NETSTANDARD2_0
            if (value is JToken)
                exceptionDetails = ((JToken)value).ToObject<ExceptionDetails>();
#endif

            return exceptionDetails != null;
        }
    }
}
