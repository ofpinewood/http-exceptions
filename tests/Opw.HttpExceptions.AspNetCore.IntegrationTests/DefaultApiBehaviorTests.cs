using FluentAssertions;
using Opw.HttpExceptions.AspNetCore._Test;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{

    public class DefaultApiBehaviorTests : IClassFixture<TestWebApplicationFactory<DefaultApiBehaviorStartup>>
    {
        private readonly HttpClient _client;

        public DefaultApiBehaviorTests(TestWebApplicationFactory<DefaultApiBehaviorStartup> factory)
        {
            _client = factory.CreateClient();
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

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.BadRequest, TestHelper.ProblemDetailsMediaTypeFormatters);
            problemDetails.Extensions.Should().HaveCount(0);
        }
    }
}
