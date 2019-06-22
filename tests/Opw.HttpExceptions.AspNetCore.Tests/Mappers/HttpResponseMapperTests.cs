using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    public class HttpResponseMapperTests
    {
        private readonly ExposeProtectedHttpResponseMapper _mapper;
        private readonly HttpContext _unauthorizedHttpContext;

        public HttpResponseMapperTests()
        {
            var optionsMock = TestHelper.CreateHttpExceptionsOptionsMock(false);
            _mapper = new ExposeProtectedHttpResponseMapper(optionsMock.Object);

            _unauthorizedHttpContext = new DefaultHttpContext();
            _unauthorizedHttpContext.Request.Path = "/api/test/unauthorized";
            _unauthorizedHttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails()
        {
            var problemDetails = _mapper.Map(_unauthorizedHttpContext.Response);

            problemDetails.ShouldNotBeNull(HttpStatusCode.Unauthorized);
            problemDetails.Instance.Should().Be("/api/test/unauthorized");

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void Map_Should_ReturnThrowArgumentOutOfRangeException_ForInvalidStatus()
        {
            _mapper.Status = 418;

            Action action = () => _mapper.Map(_unauthorizedHttpContext.Response);

            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithoutExceptionDetails()
        {
            var mapper = TestHelper.CreateHttpResponseMapper(true);
            var problemDetails = mapper.Map(_unauthorizedHttpContext.Response);

            problemDetails.ShouldNotBeNull(HttpStatusCode.Unauthorized);

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void CanMap_Should_ReturnTrue_WhenMatchingStatus()
        {
            _mapper.Status = 500;
            var result = _mapper.CanMap(500);

            result.Should().BeTrue();
        }

        [Fact]
        public void CanMap_Should_ReturnFalse_WhenNotMatchingStatus()
        {
            _mapper.Status = 500;
            var result = _mapper.CanMap(418);

            result.Should().BeFalse();
        }

        [Fact]
        public void CanMap_Should_ReturnTrue_ForAllStatuses_WhenMapperStatusIsNotSet_WhenStatusIntMinValue()
        {
            for (int i = 400; i < 600; i++)
            {
                _mapper.CanMap(i).Should().BeTrue();
            }
        }

        [Fact]
        public void MapDetail_Should_ReturnExceptionMessage()
        {
            var result = _mapper.MapDetail(_unauthorizedHttpContext.Response);

            result.Should().Be("Unauthorized");
        }

        [Fact]
        public void MapInstance_Should_ReturnRequestPath()
        {
            var result = _mapper.MapInstance(_unauthorizedHttpContext.Response);

            result.Should().Be("/api/test/unauthorized");
        }

        [Fact]
        public void MapStatus_Should_ReturnUnauthorized()
        {
            var result = _mapper.MapStatus(_unauthorizedHttpContext.Response);

            result.Should().Be((int)HttpStatusCode.Unauthorized);
        }

        [Fact]
        public void MapTitle_Should_ReturnHttpStatus()
        {
            var result = _mapper.MapTitle(_unauthorizedHttpContext.Response);

            result.Should().Be("Unauthorized");
        }

        [Fact]
        public void MapType_Should_ReturnFormattedHttpStatus()
        {
            var result = _mapper.MapType(_unauthorizedHttpContext.Response);

            result.Should().Be("error:unauthorized");
        }

        private class ExposeProtectedHttpResponseMapper : HttpResponseMapper
        {
            public ExposeProtectedHttpResponseMapper(IOptions<HttpExceptionsOptions> options) : base(options) { }

            public new string MapDetail(HttpResponse response)
            {
                return base.MapDetail(response);
            }

            public new string MapInstance(HttpResponse response)
            {
                return base.MapInstance(response);
            }

            public new int MapStatus(HttpResponse response)
            {
                return base.MapStatus(response);
            }

            public new string MapTitle(HttpResponse response)
            {
                return base.MapTitle(response);
            }

            public new string MapType(HttpResponse response)
            {
                return base.MapType(response);
            }
        }
    }
}
