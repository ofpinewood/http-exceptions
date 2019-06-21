using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpResponseExtensionsTests
    {
        [Fact]
        public void IsProblemDetailsResponse_Should_ReturnTrue_ForContentTypeProblemJson()
        {
            var responseMock = new Mock<HttpResponse>();
            responseMock.Setup(r => r.ContentType).Returns("application/problem+json; charset=utf-8");

            responseMock.Object.IsProblemDetailsResponse().Should().BeTrue();
        }

        [Fact]
        public void IsProblemDetailsResponse_Should_ReturnTrue_ForContentTypeProblemXml()
        {
            var responseMock = new Mock<HttpResponse>();
            responseMock.Setup(r => r.ContentType).Returns("application/problem+xml; charset=utf-8");

            responseMock.Object.IsProblemDetailsResponse().Should().BeTrue();
        }

        [Fact]
        public void IsProblemDetailsResponse_Should_ReturnTFalse_ForContentTypeOther()
        {
            var responseMock = new Mock<HttpResponse>();
            responseMock.Setup(r => r.ContentType).Returns("application/json; charset=utf-8");

            responseMock.Object.IsProblemDetailsResponse().Should().BeFalse();
        }
    }
}
