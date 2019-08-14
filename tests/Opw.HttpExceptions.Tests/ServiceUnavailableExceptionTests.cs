using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class ServiceUnavailableExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateServiceUnavailableException()
        {
            var exception = new ServiceUnavailableException();

            exception.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.ServiceUnavailable);
        }

        [Fact]
        public void Serialization_Should_SerializeAndDeserialize()
        {
            var exception = SerializationHelper.SerializeDeserialize(new ServiceUnavailableException());

            exception.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.ServiceUnavailable);
        }
    }
}
