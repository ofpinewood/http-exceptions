using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpExceptionsMiddlewareTests
    {
        private readonly HttpExceptionsMiddleware _middleware;
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly Mock<IOptions<HttpExceptionsOptions>> _optionsMock;
        private readonly Mock<ILogger<HttpExceptionsMiddleware>> _loggerMock;
        private readonly Mock<IActionResultExecutor<ObjectResult>> _actionResultExecutorMock;

        public HttpExceptionsMiddlewareTests()
        {
            _nextMock = new Mock<RequestDelegate>();
            _optionsMock = TestHelper.CreateHttpExceptionsOptionsMock(true);
            _actionResultExecutorMock = new Mock<IActionResultExecutor<ObjectResult>>();
            _loggerMock = new Mock<ILogger<HttpExceptionsMiddleware>>();
            _middleware = new HttpExceptionsMiddleware(_nextMock.Object, _optionsMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Invoke_Should_ReturnProblemDetailsResult_ForApplicationException_WhenExceptionThrown()
        {
            _nextMock.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Throws(new ApplicationException());
            ProblemDetailsResult result = null;
            _actionResultExecutorMock.Setup(e => e.ExecuteAsync(It.IsAny<ActionContext>(), It.IsAny<ObjectResult>()))
                .Callback<ActionContext, ObjectResult>((actionContext, actionResult) => result = (ProblemDetailsResult)actionResult)
                .Returns(Task.CompletedTask);

            var services = new ServiceCollection();
            services.AddTransient((_) => _actionResultExecutorMock.Object);
            var context = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider()
            };

            await _middleware.Invoke(context);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            result.Value.ShouldNotBeNull(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Invoke_Should_ReturnProblemDetailsResult_WhenUnauthorizedRequest()
        {
            _nextMock.Setup(n => n.Invoke(It.IsAny<HttpContext>()))
                .Returns((HttpContext ctx) => {
                    ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return Task.CompletedTask;
                });

            ProblemDetailsResult result = null;
            _actionResultExecutorMock.Setup(e => e.ExecuteAsync(It.IsAny<ActionContext>(), It.IsAny<ObjectResult>()))
                .Callback<ActionContext, ObjectResult>((actionContext, actionResult) => result = (ProblemDetailsResult)actionResult)
                .Returns(Task.CompletedTask);

            var services = new ServiceCollection();
            services.AddTransient((_) => _actionResultExecutorMock.Object);
            var context = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider()
            };

            await _middleware.Invoke(context);

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.Unauthorized);
            result.Value.ShouldNotBeNull(HttpStatusCode.Unauthorized);
            result.Value.Title.Should().Be("Unauthorized");
        }

        [Fact]
        public async Task Invoke_Should_LogError_WithDefaultShouldLogExceptionConstraints()
        {
            //var options = _optionsMock.Object.Value;
            //options.ShouldLogException = (exception) => {
            //    return true;
            //};
            //_optionsMock.Setup(o => o.Value).Returns(options);

            _nextMock.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Throws(new ApplicationException());
            ProblemDetailsResult result = null;
            _actionResultExecutorMock.Setup(e => e.ExecuteAsync(It.IsAny<ActionContext>(), It.IsAny<ObjectResult>()))
                .Callback<ActionContext, ObjectResult>((actionContext, actionResult) => result = (ProblemDetailsResult)actionResult)
                .Returns(Task.CompletedTask);

            var services = new ServiceCollection();
            services.AddTransient((_) => _actionResultExecutorMock.Object);
            var context = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider()
            };

            await _middleware.Invoke(context);

            _loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        [Fact]
        public async Task Invoke_Should_NotLogError_WithCustomShouldLogExceptionConstraints()
        {
            var options = _optionsMock.Object.Value;
            options.ShouldLogException = (_) => false;
            _optionsMock.Setup(o => o.Value).Returns(options);

            _nextMock.Setup(n => n.Invoke(It.IsAny<HttpContext>())).Throws(new ApplicationException());
            ProblemDetailsResult result = null;
            _actionResultExecutorMock.Setup(e => e.ExecuteAsync(It.IsAny<ActionContext>(), It.IsAny<ObjectResult>()))
                .Callback<ActionContext, ObjectResult>((actionContext, actionResult) => result = (ProblemDetailsResult)actionResult)
                .Returns(Task.CompletedTask);

            var services = new ServiceCollection();
            services.AddTransient((_) => _actionResultExecutorMock.Object);
            var context = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider()
            };

            await _middleware.Invoke(context);

            _loggerMock.Verify(l => l.Log(
                It.IsAny<LogLevel>(),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Never);
        }
    }
}
