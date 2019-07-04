using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
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

        public static ExceptionInfo ShouldHaveExceptionInfo(this ProblemDetails problemDetails)
        {
            problemDetails.TryGetExceptionInfo(out var exceptionInfo).Should().BeTrue();

            exceptionInfo.Should().NotBeNull();
            exceptionInfo.Source.Should().NotBeNull();
            exceptionInfo.StackTrace.Should().NotBeNull();

            return exceptionInfo;
        }

        public static bool TryGetExceptionInfo(this ProblemDetails problemDetails, out ExceptionInfo exceptionInfo)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ExceptionInfo).ToCamelCase(), out var value))
                return value.TryParseExceptionInfo(out exceptionInfo);

            exceptionInfo = null;
            return false;
        }

        public static bool TryParseExceptionInfo(this object value, out ExceptionInfo exceptionInfo)
        {
            exceptionInfo = null;

#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
            if (value is ExceptionInfo)
                exceptionInfo = (ExceptionInfo)value;
            if (value is JToken)
                exceptionInfo = ((JToken)value).ToObject<ExceptionInfo>();
#pragma warning restore RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.

            return exceptionInfo != null;
        }
    }
}
