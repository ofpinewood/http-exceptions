using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateNotFoundException()
        {
            var exception = new NotFoundException();

            exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.NotFound);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new NotFoundException());

            exception.StatusCode.Should().Be(HttpStatusCode.NotFound);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.NotFound);
        }
    }
}
