using FluentAssertions;
using Opw.HttpExceptions.AspNetCore._Test;
using System;
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
            _ = factory ?? throw new ArgumentNullException(nameof(factory));

            _client = factory.CreateClient();
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
            problemDetails.Title.Should().Be("InvalidModel");
            problemDetails.Extensions.Should().HaveCount(1);
        }

        [Fact]
        public async Task Authorized_Should_ReturnProblemDetails_UsingHttpResponseMappers()
        {
            var response = await _client.GetAsync("test/authorized");

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.Unauthorized);
            problemDetails.Title.Should().Be("Unauthorized");
            problemDetails.Extensions.Should().HaveCount(0);
        }
    }
}
