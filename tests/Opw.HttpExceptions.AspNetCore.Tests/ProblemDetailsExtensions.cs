using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class ProblemDetailsExtensions
    {
        public static ProblemDetails ShouldNotBeNull(this ProblemDetails problemDetails, HttpStatusCode statusCode)
        {
            problemDetails.Should().NotBeNull();
            problemDetails.Status.Should().Be((int)statusCode);
            problemDetails.Title.Should().NotBeNull();
            problemDetails.Detail.Should().NotBeNull();
            problemDetails.Type.Should().NotBeNull();

            return problemDetails;
        }

        public static ExceptionDetails ShouldHaveExceptionDetails(this ProblemDetails problemDetails)
        {
            problemDetails.TryGetExceptionDetails(out var exceptionDetails).Should().BeTrue();

            exceptionDetails.Should().NotBeNull();
            exceptionDetails.Name.Should().NotBeNull();
            exceptionDetails.Source.Should().NotBeNull();
            exceptionDetails.StackTrace.Should().NotBeNull();

            return exceptionDetails;
        }

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

#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
            if (value is ExceptionDetails)
                exceptionDetails = (ExceptionDetails)value;
            if (value is JToken)
                exceptionDetails = ((JToken)value).ToObject<ExceptionDetails>();
#pragma warning restore RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.

            return exceptionDetails != null;
        }
    }
}
