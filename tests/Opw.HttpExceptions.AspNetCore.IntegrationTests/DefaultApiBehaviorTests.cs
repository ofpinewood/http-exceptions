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
        public async Task Get_Should_ReturnProblemDetails()
        {
            var product = new Product();
            var response = await _client.PostAsJsonAsync("test/product", product);

            var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.BadRequest, TestHelper.ProblemDetailsMediaTypeFormatters);
            problemDetails.Extensions.Should().HaveCount(0);
        }
    }
}
