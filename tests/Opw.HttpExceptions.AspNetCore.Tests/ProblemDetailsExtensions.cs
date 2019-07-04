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

        public static TException ShouldHaveException<TException>(this ProblemDetails problemDetails)
            where TException : Exception
        {
            problemDetails.TryGetException<TException>(out var exception).Should().BeTrue();

            exception.Should().NotBeNull();
            exception.Source.Should().NotBeNull();
            exception.StackTrace.Should().NotBeNull();

            return exception;
        }

        public static bool TryGetException<TException>(this ProblemDetails problemDetails, out TException exception)
            where TException : Exception
        {
            if (problemDetails.Extensions.TryGetValue(nameof(Exception).ToCamelCase(), out var value))
                return value.TryParseException(out exception);

            exception = null;
            return false;
        }

        internal static bool TryParseException<TException>(this object value, out TException exception)
            where TException : Exception
        {
            exception = null;

#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
            if (value is TException)
                exception = (TException)value;
            if (value is JToken)
                exception = ((JToken)value).ToObject<TException>();
#pragma warning restore RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.

            return exception != null;
        }
    }
}
