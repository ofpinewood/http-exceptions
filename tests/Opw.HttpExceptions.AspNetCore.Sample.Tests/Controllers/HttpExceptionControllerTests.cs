using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Net;
using System.Linq;
using System;
using FluentAssertions;
using Opw.HttpExceptions.AspNetCore.Sample.Models;
using Opw.HttpExceptions.AspNetCore.Sample.CustomErrors;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    public class HttpExceptionControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly TestWebApplicationFactory _factory;
        private HttpClient _client;

        public HttpExceptionControllerTests(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Throw_Should_ReturnProblemDetails()
        {
            foreach (var statusCode in Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Where(c => (int)c >= 400 && (int)c < 600))
            {
                var response = await _client.GetAsync($"test/{statusCode}");

                var problemDetails = response.ShouldBeProblemDetails(statusCode);
                problemDetails.Extensions.Should().HaveCount(0);
            }
        }

        //[Fact]
        //public async Task Throw_Should_ReturnProblemDetails_WithExceptionDetails()
        //{
        //    TestHelper.SetHostEnvironmentName(_factory.Server.Host, "Development");
        //    _client = _factory.CreateClient();

        //    foreach (var statusCode in Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().Where(c => (int)c >= 400 && (int)c < 600))
        //    {
        //        var response = await _client.GetAsync($"test/{statusCode}");

        //        var problemDetails = response.ShouldBeProblemDetails(statusCode);
        //        problemDetails.Extensions.Should().HaveCount(1);

        //        var exception = problemDetails.ShouldHaveExceptionDetails();
        //        exception.Type.Should().Be(nameof(HttpException));
        //        exception.InnerException.Should().NotBeNull();

        //        var result = exception.InnerException.TryParseSerializableException(out var innerException);
        //        result.Should().BeTrue();
        //        innerException.Type.Should().Be(nameof(ApplicationException));
        //    }

        //    // reset the EnvironmentName back to production
        //    TestHelper.SetHostEnvironmentName(_factory.Server.Host, "Production");
        //}

        [Fact]
        public async Task ThrowApplicationException_Should_ReturnProblemDetails()
        {
            var response = await _client.GetAsync("test/applicationException");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.InternalServerError);
            problemDetails.Title.Should().Be("Application");
            problemDetails.Type.Should().Be("error:application");
            problemDetails.Extensions.Should().HaveCount(0);
        }

        [Fact]
        public async Task ThrowProblemDetailsAttributeException_Should_ReturnProblemDetails()
        {
            var response = await _client.GetAsync("test/problemDetailsAttributeException");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.Ambiguous);
            problemDetails.Title.Should().Be("ProblemDetailsAttribute");
            problemDetails.Type.Should().Be(ResponseStatusCodeLink.InternalServerError);
            problemDetails.Extensions.Should().HaveCount(2);
            problemDetails.Extensions[nameof(ProblemDetailsAttributeException.PropertyA).ToCamelCase()].ToString().Should().Be("AAA");
            problemDetails.Extensions[nameof(ProblemDetailsAttributeException.PropertyB).ToCamelCase()].ToString().Should().Be("42");
            problemDetails.Extensions.Should().NotContainKey(nameof(ProblemDetailsAttributeException.PropertyC).ToCamelCase());
        }

        [Fact]
        public async Task PostProduct_Should_ReturnOk()
        {
            var product = new Product { Id = "1" };
            var response = await _client.PostAsync("test/product", product.ToJsonContent());

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task PostProduct_Should_ReturnProblemDetails()
        {
            var product = new Product();
            var response = await _client.PostAsync("test/product", product.ToJsonContent());

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.BadRequest);
            problemDetails.Extensions.Should().HaveCount(1);
        }

        [Fact]
        public async Task Authorized_Should_ReturnProblemDetails_UsingHttpResponseMappers()
        {
            var response = await _client.GetAsync("test/authorized");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.Unauthorized);
            problemDetails.Extensions.Should().HaveCount(0);
        }

        //[Fact]
        //public async Task Throw_Should_ReturnCustomError()
        //{
        //    var response = await _client.GetAsync("test/customError");

        //    response.StatusCode.Should().Be(418);
        //    response.Content.Headers.ContentType.MediaType.Should().Be("application/problem+json");

        //    var customError = response.Content.ReadAsAsync<CustomError>().Result;

        //    customError.Should().NotBeNull();
        //    customError.Status.Should().Be(418);
        //    customError.Type.Should().NotBeNull();
        //    customError.Message.Should().NotBeNull();
        //    customError.Code.Should().Be(42);
        //}

        [Fact]
        public async Task Throw_Should_ReturnCustomErrorAsXml()
        {
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));
            var response = await _client.GetAsync("test/customError");

            response.StatusCode.Should().Be(418);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/problem+xml");
        }
    }
}
