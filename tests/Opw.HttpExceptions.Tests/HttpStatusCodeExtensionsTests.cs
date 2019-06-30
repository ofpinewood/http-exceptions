using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class HttpStatusCodeExtensionsTests
    {
        [Fact]
        public void TryGetLink_Should_ReturnLink_ForInternalServerError()
        {
            var result = HttpStatusCode.InternalServerError.TryGetLink(out var link);

            result.Should().BeTrue();
            link.Should().Be(ResponseStatusCodeLink.InternalServerError);
        }

        [Fact]
        public void TryGetLink_Should_ReturnFalse_ForOK()
        {
            var result = HttpStatusCode.OK.TryGetLink(out var link);

            result.Should().BeFalse();
        }
    }
}
