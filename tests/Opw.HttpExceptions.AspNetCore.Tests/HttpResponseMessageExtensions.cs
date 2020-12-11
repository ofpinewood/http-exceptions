using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class HttpResponseMessageExtensions
    {
        public static ProblemDetails ShouldBeProblemDetails(this HttpResponseMessage response, HttpStatusCode statusCode)
        {
            response.StatusCode.Should().Be(statusCode);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/problem+json");

            var problemDetails = response.Content.ReadAsProblemDetails();

            problemDetails.Should().NotBeNull();
            problemDetails.Status.Should().Be((int)statusCode);
            problemDetails.Title.Should().NotBeNull();
            problemDetails.Type.Should().NotBeNull();
            problemDetails.Detail.Should().NotBeNull();
            problemDetails.Instance.Should().Be(response.RequestMessage.RequestUri.AbsolutePath);

            return problemDetails;
        }
    }
}
