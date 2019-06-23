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
        }
    }
}
