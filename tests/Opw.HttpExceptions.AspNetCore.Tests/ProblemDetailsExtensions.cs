using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class ProblemDetailsExtensions
    {
        public static ProblemDetails ShouldNotBeNull(this ProblemDetails problemDetails, HttpStatusCode statusCode)
        {
            _ = problemDetails ?? throw new ArgumentNullException(nameof(problemDetails));

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
    }
}
