using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using System.Linq;
using System.Net.Http.Formatting;
using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    public class HttpExceptionControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private HttpClient _client;
        private readonly IEnumerable<MediaTypeFormatter> _problemDetailsMediaTypeFormatters;

        public HttpExceptionControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            _problemDetailsMediaTypeFormatters = factory.ProblemDetailsMediaTypeFormatters;
        }

        [Fact]
        public async Task Throw_Should_ReturnProblemDetails()
        {
            foreach (var statusCode in Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Where(c => (int)c >= 400 && (int)c < 600))
            {
                var response = await _client.GetAsync($"httpexception/{statusCode}");

                var problemDetails = response.ShouldBeProblemDetails(statusCode, _problemDetailsMediaTypeFormatters);
                problemDetails.Extensions.Should().HaveCount(0);
            }
        }

        [Fact]
        public async Task Throw_Should_ReturnProblemDetails_WithExceptionDetails()
        {
            _factory.Server.Host.Services.GetRequiredService<IHostingEnvironment>().EnvironmentName = "development";
            _client = _factory.CreateClient();

            foreach (var statusCode in Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Where(c => (int)c >= 400 && (int)c < 600))
            {
                var response = await _client.GetAsync($"httpexception/{statusCode}");

                var problemDetails = response.ShouldBeProblemDetails(statusCode, _problemDetailsMediaTypeFormatters);
                problemDetails.Extensions.Should().HaveCount(1);

                var exceptionDetails = problemDetails.ShouldHaveExceptionDetails();
                exceptionDetails.Name.Should().Be(nameof(HttpException));
                exceptionDetails.InnerException.Should().NotBeNull();
                exceptionDetails.InnerException.Name.Should().Be(nameof(ApplicationException));
            }
        }

        [Fact]
        public async Task ThrowApplicationException_Should_ReturnProblemDetails()
        {
            var response = await _client.GetAsync($"httpexception/applicationException");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.InternalServerError, _problemDetailsMediaTypeFormatters);
            problemDetails.Title.Should().Be("Application");
            problemDetails.Type.Should().Be("error:application");
            problemDetails.Extensions.Should().HaveCount(0);
        }
    }
}
