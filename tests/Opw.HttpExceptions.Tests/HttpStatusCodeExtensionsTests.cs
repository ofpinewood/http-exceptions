using FluentAssertions;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions
{
    public class HttpStatusCodeExtensionsTests
    {
        [Fact]
        public void TryGetInformationLink_Should_ReturnLink_ForInternalServerError()
        {
            var result = HttpStatusCode.InternalServerError.TryGetInformationLink(out var link);

            result.Should().BeTrue();
            link.Should().Be(ResponseStatusCodeLink.InternalServerError);
        }

        [Fact]
        public void TryGetInformationLink_Should_ReturnLink_ForInternalServerErrorAsInt()
        {
            var result = ((int)HttpStatusCode.InternalServerError).TryGetInformationLink(out var link);

            result.Should().BeTrue();
            link.Should().Be(ResponseStatusCodeLink.InternalServerError);
        }

        [Fact]
        public void TryGetInformationLink_Should_ReturnFalse_ForOK()
        {
            var result = HttpStatusCode.OK.TryGetInformationLink(out var link);

            result.Should().BeFalse();
        }
    }
}
