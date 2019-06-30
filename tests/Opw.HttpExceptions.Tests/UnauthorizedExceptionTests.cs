using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class UnauthorizedExceptionTests
    {
        [Fact]
        public void Constructor_Should_CreateUnauthorizedException()
        {
            var exception = new UnauthorizedException();

            exception.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            exception.HelpLink.Should().Be(ResponseStatusCodeLink.Unauthorized);
        }
    }
}
