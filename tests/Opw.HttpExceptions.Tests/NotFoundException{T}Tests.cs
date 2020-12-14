using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
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

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new NotFoundException<Product>());

            exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.NotFound);
        }

        [SuppressMessage("Style", "CA1812:Avoid uninstantiated internal classes", Justification = "Used in the tests.")]
        private class Product
        {
            public string Id { get; set; }
        }
    }
}
