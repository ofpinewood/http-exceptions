using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using Xunit;
using Moq;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpExceptionsOptionsTests
    {
        private readonly Mock<IOptions<HttpExceptionsOptions>> _httpExceptionsOptionsMock;
        private readonly DefaultHttpContext _internalServerErrorHttpContext;

        public HttpExceptionsOptionsTests()
        {
            _httpExceptionsOptionsMock = TestHelper.CreateHttpExceptionsOptionsMock(true);

            _internalServerErrorHttpContext = new DefaultHttpContext();
            _internalServerErrorHttpContext.Request.Path = "/api/test/internal-server-error";
            _internalServerErrorHttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        [Fact]
        public void TryMap_Should_ReturnTrue_WhenMappingConfiguredException()
        {
            var options = new HttpExceptionsOptions();
            options.ExceptionMappers.Add(new ProblemDetailsExceptionMapper<HttpException>(_httpExceptionsOptionsMock.Object));

            var result = options.TryMap(new HttpException(), new DefaultHttpContext(), out _);

            result.Should().BeTrue();
        }

        [Fact]
        public void TryMap_Should_ReturnFalse_WhenMappingNotConfiguredException()
        {
            var options = new HttpExceptionsOptions();
            options.ExceptionMappers.Add(new ProblemDetailsExceptionMapper<HttpException>(_httpExceptionsOptionsMock.Object));

            var result = options.TryMap(new ArgumentException(), new DefaultHttpContext(), out _);

            result.Should().BeFalse();
        }

        [Fact]
        public void TryMap_Should_ReturnTrue_WhenMappingConfiguredHttpResponse()
        {
            var options = new HttpExceptionsOptions();
            options.HttpResponseMappers.Add(new ProblemDetailsHttpResponseMapper(_httpExceptionsOptionsMock.Object) { Status = 500 });

            var result = options.TryMap(_internalServerErrorHttpContext.Response, out _);

            result.Should().BeTrue();
        }

        [Fact]
        public void TryMap_Should_ReturnFalse_WhenMappingNotConfiguredHttpResponse()
        {
            var options = new HttpExceptionsOptions();
            options.HttpResponseMappers.Add(new ProblemDetailsHttpResponseMapper(_httpExceptionsOptionsMock.Object) { Status = 148 });

            var result = options.TryMap(_internalServerErrorHttpContext.Response, out _);

            result.Should().BeFalse();
        }
    }
}
