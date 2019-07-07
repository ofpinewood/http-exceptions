using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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

        public static SerializableException ShouldHaveExceptionDetails(this ProblemDetails problemDetails)
        {
            problemDetails.TryGetExceptionDetails(out var exception).Should().BeTrue();

            exception.Should().NotBeNull();
            exception.Source.Should().NotBeNull();
            exception.StackTrace.Should().NotBeNull();

            return exception;
        }

        public static bool TryGetExceptionDetails(this ProblemDetails problemDetails, out SerializableException exception)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ProblemDetailsExtensionMembers.ExceptionDetails).ToCamelCase(), out var value))
                return value.TryParseSerializableException(out exception);

            exception = null;
            return false;
        }

        public static bool TryParseSerializableException(this object value, out SerializableException exception)
        {
            exception = null;

#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
            if (value is SerializableException)
                exception = (SerializableException)value;
            if (value is JToken)
                exception = ((JToken)value).ToObject<SerializableException>();
#pragma warning restore RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.

            return exception != null;
        }

        public static bool TryGetErrors(this ProblemDetails problemDetails, out IDictionary<string, object[]> errors)
        {
            if (problemDetails.Extensions.TryGetValue(nameof(ProblemDetailsExtensionMembers.Errors).ToCamelCase(), out var value))
                return value.TryParseErrors(out errors);

            errors = null;
            return false;
        }

        public static bool TryParseErrors(this object value, out IDictionary<string, object[]> errors)
        {
            errors = null;

#pragma warning disable RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.
            if (value is IDictionary<string, object[]>)
                errors = (IDictionary<string, object[]>)value;
            if (value is JToken)
                errors = ((JToken)value).ToObject<IDictionary<string, object[]>>();
#pragma warning restore RCS1220 // Use pattern matching instead of combination of 'is' operator and cast operator.

            return errors != null;
        }
    }
}
