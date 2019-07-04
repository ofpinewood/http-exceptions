using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class UnsupportedMediaTypeExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateUnsupportedMediaTypeException()
        {
            var exception = new UnsupportedMediaTypeException();

            exception.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.UnsupportedMediaType);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new UnsupportedMediaTypeException());

            exception.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.UnsupportedMediaType);
        }
    }
}
