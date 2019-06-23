using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions.AspNetCore._Test;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class NotUseInvalidModelStateResponseFactoryTests : IClassFixture<TestWebApplicationFactory<NotUseInvalidModelStateResponseFactoryStartup>>
    {
        private readonly HttpClient _client;

        public NotUseInvalidModelStateResponseFactoryTests(TestWebApplicationFactory<NotUseInvalidModelStateResponseFactoryStartup> factory)
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
        public async Task PostProduct_Should_ReturnProblemDetails_UsingAspNetCoreDefaultImplementation()
        {
            var product = new Product();
            var response = await _client.PostAsJsonAsync("test/product", product);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/problem+json");

            var problemDetails = response.Content.ReadAsAsync<ProblemDetails>(TestHelper.ProblemDetailsMediaTypeFormatters).Result;

            problemDetails.Should().NotBeNull();
            problemDetails.Status.Should().Be((int)HttpStatusCode.BadRequest);
            problemDetails.Title.Should().NotBeNull();
            problemDetails.Type.Should().BeNull();
            problemDetails.Detail.Should().BeNull();
            problemDetails.Instance.Should().BeNull();
            problemDetails.Extensions.Should().HaveCount(2);
            problemDetails.Extensions.ContainsKey("traceId").Should().BeTrue();
        }
    }
}
