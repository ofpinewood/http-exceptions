using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
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
    }
}
