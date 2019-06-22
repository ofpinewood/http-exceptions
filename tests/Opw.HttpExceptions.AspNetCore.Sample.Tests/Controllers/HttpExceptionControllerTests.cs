using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using System.Linq;
using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Opw.HttpExceptions.AspNetCore.Sample.Models;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    public class HttpExceptionControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private HttpClient _client;

        public HttpExceptionControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Throw_Should_ReturnProblemDetails()
        {
            foreach (var statusCode in Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Where(c => (int)c >= 400 && (int)c < 600))
            {
                var response = await _client.GetAsync($"test/{statusCode}");

                var problemDetails = response.ShouldBeProblemDetails(statusCode, TestHelper.ProblemDetailsMediaTypeFormatters);
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
                var response = await _client.GetAsync($"test/{statusCode}");

                var problemDetails = response.ShouldBeProblemDetails(statusCode, TestHelper.ProblemDetailsMediaTypeFormatters);
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
            var response = await _client.GetAsync("test/applicationException");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.InternalServerError, TestHelper.ProblemDetailsMediaTypeFormatters);
            problemDetails.Title.Should().Be("Application");
            problemDetails.Type.Should().Be("error:application");
            problemDetails.Extensions.Should().HaveCount(0);
        }

        [Fact]
        public async Task PostProduct_Should_ReturnOk()
        {
            var product = new Product { Id = "1" };
            var response = await _client.PostAsJsonAsync("test/product", product);

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostProduct_Should_ReturnProblemDetails()
        {
            var product = new Product();
            var response = await _client.PostAsJsonAsync("test/product", product);

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.BadRequest, TestHelper.ProblemDetailsMediaTypeFormatters);
            problemDetails.Extensions.Should().HaveCount(0);
        }

        [Fact]
        public async Task Authorized_Should_ReturnProblemDetails_UsingHttpResponseMappers()
        {
            var response = await _client.GetAsync("test/authorized");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.Unauthorized, TestHelper.ProblemDetailsMediaTypeFormatters);
            problemDetails.Extensions.Should().HaveCount(0);
        }
    }
}
