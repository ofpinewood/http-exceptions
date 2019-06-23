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
        }
    }
}
