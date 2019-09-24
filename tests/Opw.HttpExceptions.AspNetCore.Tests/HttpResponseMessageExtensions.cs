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

#if NETCOREAPP2_2
            var problemDetails = response.Content.ReadAsAsync<ProblemDetails>().Result;
#endif
#if NETCOREAPP3_0
            var str = response.Content.ReadAsStringAsync().Result;
            var converter = new ProblemDetailsJsonConverter();
            var reader = new System.Text.Json.Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(str));
            var problemDetails = converter.Read(ref reader, typeof(ProblemDetails), new JsonOptions().JsonSerializerOptions);
#endif

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
