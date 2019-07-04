using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class ConflictExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateConflictException()
        {
            var exception = new ConflictException();

            exception.StatusCode.Should().Be(HttpStatusCode.Conflict);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.Conflict);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new ConflictException());

            exception.StatusCode.Should().Be(HttpStatusCode.Conflict);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.Conflict);
        }
    }
}
