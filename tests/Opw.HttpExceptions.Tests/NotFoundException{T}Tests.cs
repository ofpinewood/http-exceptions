using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class NotFoundException_T_Tests
    {
        [Fact]
        public void Constructor_Should_CreateNotFoundException()
        {
            var exception = new NotFoundException<Product>();

            exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.NotFound);
        }

        private class Product
        {
            public string Id { get; set; }
        }
    }
}
