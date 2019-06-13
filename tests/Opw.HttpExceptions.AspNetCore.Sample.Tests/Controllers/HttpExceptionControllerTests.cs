using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Net;
using Opw.HttpExceptions.AspNetCore.Sample.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Formatting;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    public class HttpExceptionControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IEnumerable<MediaTypeFormatter> _problemDetailsMediaTypeFormatters;

        public HttpExceptionControllerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _problemDetailsMediaTypeFormatters = factory.ProblemDetailsMediaTypeFormatters;
        }

        [Fact]
        public async Task Get_Should_ReturnBadRequest()
        {
            var response = await _client.GetAsync($"httpexception/{HttpExceptionType.BadRequest}");
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/problem+json");
            
            var problemDetails = await response.Content.ReadAsAsync<ProblemDetails>(_problemDetailsMediaTypeFormatters);

            problemDetails.Should().NotBeNull();
            problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
