using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class ExceptionMapperTests
    {
        private readonly ExposeProtectedExceptionMapper _mapper;

        public ExceptionMapperTests()
        {
            var optionsMock = TestsHelper.CreateHttpExceptionsOptionsMock(false);
            _mapper = new ExposeProtectedExceptionMapper(optionsMock.Object);
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails()
        {
            var problemDetails = _mapper.Map(new ApplicationException(), new DefaultHttpContext());

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetails.Instance.Should().BeNull();

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithHelpLink()
        {
            var helpLink = "https://docs.microsoft.com/en-us/dotnet/api/system.exception.helplink?view=netcore-2.2";
            var problemDetails = _mapper.Map(new ApplicationException { HelpLink = helpLink }, new DefaultHttpContext());

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetails.Instance.Should().Be(helpLink);

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeFalse();
            exceptionDetails.Should().BeNull();
        }

        [Fact]
        public void Map_Should_ReturnProblemDetails_WithExceptionDetails()
        {
            var mapper = TestsHelper.CreateExceptionMapper<Exception>(true);
            var problemDetails = mapper.Map(new ApplicationException(), new DefaultHttpContext());

            problemDetails.ShouldNotBeNull(HttpStatusCode.InternalServerError);
            problemDetails.Instance.Should().BeNull();

            var result = problemDetails.TryGetExceptionDetails(out var exceptionDetails);

            result.Should().BeTrue();
            exceptionDetails.Should().NotBeNull();
        }

        [Fact]
        public void MapDetail_Should_ReturnExceptionMessage()
        {
            var message = "Test exception message.";
            var exception = new ApplicationException(message);
            var result = _mapper.MapDetail(exception, new DefaultHttpContext());

            result.Should().Be(message);
        }

        [Fact]
        public void MapInstance_Should_ReturnExceptionHelpLink()
        {
            var helpLink = "https://docs.microsoft.com/en-us/dotnet/api/system.exception.helplink?view=netcore-2.2";
            var exception = new ApplicationException { HelpLink = helpLink };
            var result = _mapper.MapInstance(exception, new DefaultHttpContext());

            result.Should().Be(helpLink);
        }

        [Fact]
        public void MapInstance_Should_ReturnRequestPath()
        {
            var exception = new ApplicationException();
            var context = new DefaultHttpContext();
            var requestPath = "/test/123";
            context.Request.Path = requestPath;
            var result = _mapper.MapInstance(exception, context);

            result.Should().Be(requestPath);
        }

        [Fact]
        public void MapStatus_Should_ReturnInternalServerError()
        {
            var exception = new ApplicationException();
            var result = _mapper.MapStatus(exception, new DefaultHttpContext());

            result.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void MapStatus_Should_ReturnBadRequest()
        {
            var exception = new HttpException(HttpStatusCode.BadRequest);
            var result = _mapper.MapStatus(exception, new DefaultHttpContext());

            result.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public void MapTitle_Should_ReturnFormattedExceptionName()
        {
            var exception = new DivideByZeroException();
            var result = _mapper.MapTitle(exception, new DefaultHttpContext());

            result.Should().Be("DivideByZero");
        }

        [Fact]
        public void MapType_Should_ReturnFormattedExceptionName()
        {
            var exception = new DivideByZeroException();
            var result = _mapper.MapType(exception, new DefaultHttpContext());

            result.Should().Be("error:divide-by-zero");
        }

        private class ExposeProtectedExceptionMapper : ExceptionMapper<Exception>
        {
            public ExposeProtectedExceptionMapper(IOptions<HttpExceptionsOptions> options) : base(options) { }

            public new string MapDetail(Exception exception, HttpContext context)
            {
                return base.MapDetail(exception, context);
            }

            public new string MapInstance(Exception exception, HttpContext context)
            {
                return base.MapInstance(exception, context);
            }

            public new int MapStatus(Exception exception, HttpContext context)
            {
                return base.MapStatus(exception, context);
            }

            public new string MapTitle(Exception exception, HttpContext context)
            {
                return base.MapTitle(exception, context);
            }

            public new string MapType(Exception exception, HttpContext context)
            {
                return base.MapType(exception, context);
            }
        }
    }
}
