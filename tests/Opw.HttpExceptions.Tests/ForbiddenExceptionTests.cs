using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class ForbiddenExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateForbiddenException()
        {
            var exception = new ForbiddenException();

            exception.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.Forbidden);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new ForbiddenException());

            exception.StatusCode.Should().Be(HttpStatusCode.Forbidden);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.Forbidden);
        }
    }
}
