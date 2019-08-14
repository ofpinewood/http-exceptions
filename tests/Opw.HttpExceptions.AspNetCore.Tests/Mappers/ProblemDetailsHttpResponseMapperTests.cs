using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    public class ProblemDetailsHttpResponseMapperTests
    {
        private readonly ExposeProtectedProblemDetailsHttpResponseMapper _mapper;
        private readonly HttpContext _unauthorizedHttpContext;

        public ProblemDetailsHttpResponseMapperTests()
        {
            var optionsMock = TestHelper.CreateHttpExceptionsOptionsMock(false);
            _mapper = new ExposeProtectedProblemDetailsHttpResponseMapper(optionsMock.Object);

            _unauthorizedHttpContext = new DefaultHttpContext();
            _unauthorizedHttpContext.Request.Path = "/api/test/unauthorized";
            _unauthorizedHttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails()
        {
            var actionResult = _mapper.Map(_unauthorizedHttpContext.Response);

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.Unauthorized);
            problemDetailsResult.Value.Instance.Should().Be("/api/test/unauthorized");

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeFalse();
            exception.Should().BeNull();
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
            var mapper = TestHelper.CreateProblemDetailsHttpResponseMapper(true);
            var actionResult = mapper.Map(_unauthorizedHttpContext.Response);

            actionResult.Should().BeOfType<ProblemDetailsResult>();
            var problemDetailsResult = (ProblemDetailsResult)actionResult;

            problemDetailsResult.Value.ShouldNotBeNull(HttpStatusCode.Unauthorized);

            var result = problemDetailsResult.Value.TryGetExceptionDetails(out var exception);

            result.Should().BeFalse();
            exception.Should().BeNull();
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

        private class ExposeProtectedProblemDetailsHttpResponseMapper : ProblemDetailsHttpResponseMapper
        {
            public ExposeProtectedProblemDetailsHttpResponseMapper(IOptions<HttpExceptionsOptions> options) : base(options) { }

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
