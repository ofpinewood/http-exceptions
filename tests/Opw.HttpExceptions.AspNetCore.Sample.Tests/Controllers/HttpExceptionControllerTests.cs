using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using System.Linq;
using System.Net;
using Opw.HttpExceptions.AspNetCore.Sample.Models;

namespace Opw.HttpExceptions.AspNetCore.Sample.Controllers
{
    public class HttpExceptionControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public HttpExceptionControllerTests(TestWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_Should_ReturnBadRequest()
        {
            var response = await _client.GetAsync($"httpexception/{HttpExceptionType.BadRequest}");
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
