using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class HttpExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateHttpException_WithStatusCodeInternalServerError()
        {
            var exception = new HttpException();

            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.InternalServerError);
        }

        [Fact]
        public void Constructor_Should_CreateHttpException_WithStatusCodeBadRequest()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest);

            exception.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.BadRequest);
        }
    }
}
