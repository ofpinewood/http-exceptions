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
        }
    }
}
