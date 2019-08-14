using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class BadRequestExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateBadRequestException()
        {
            var exception = new BadRequestException();

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new BadRequestException());

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
        }
    }
}
