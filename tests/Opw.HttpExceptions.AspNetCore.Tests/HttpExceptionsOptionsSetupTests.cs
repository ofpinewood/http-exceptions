using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;
using Moq;
using System.Linq;
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

    public class HttpExceptionsOptionsSetupTests
    {
        [Fact]
        public void Configure_Should_ConfigureExceptionMappers()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o =>
            {
                o.ExceptionMapper<HttpException, ProblemDetailsExceptionMapper<HttpException>>();
                o.ExceptionMapper<Exception, ProblemDetailsExceptionMapper<Exception>>();
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.ExceptionMapperDescriptors.Should().HaveCount(2);
            options.Value.ExceptionMappers.Should().HaveCount(2);
        }

        [Fact]
        public void Configure_Should_ConfigureExceptionMappers_InCorrectOrder()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o =>
            {
                o.ExceptionMapper<HttpException, ProblemDetailsExceptionMapper<HttpException>>();
                o.ExceptionMapper<ArgumentException, ProblemDetailsExceptionMapper<ArgumentException>>();
                o.ExceptionMapper<Exception, TestProblemDetailsExceptionMapper>();
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.ExceptionMapperDescriptors.Should().HaveCount(3);

            var exceptionMappers = options.Value.ExceptionMappers.ToArray();
            exceptionMappers.Should().HaveCount(3);
            exceptionMappers[0].Should().BeOfType<ProblemDetailsExceptionMapper<HttpException>>();
            exceptionMappers[1].Should().BeOfType<ProblemDetailsExceptionMapper<ArgumentException>>();
            exceptionMappers[2].Should().BeOfType<TestProblemDetailsExceptionMapper>();
        }

        [Fact]
        public void Configure_Should_ConfigureHttpResponseMappers()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o =>
            {
                o.HttpResponseMapper<TestProblemDetailsHttpResponseMapper>(500);
                o.HttpResponseMapper<ProblemDetailsHttpResponseMapper>();
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.HttpResponseMapperDescriptors.Should().HaveCount(2);
            options.Value.HttpResponseMappers.Should().HaveCount(2);
        }

        [Fact]
        public void Configure_Should_ConfigureHttpResponseMappersWithCorrectStatus()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o =>
            {
                o.HttpResponseMapper<TestProblemDetailsHttpResponseMapper>(418);
                o.HttpResponseMapper<ProblemDetailsHttpResponseMapper>();
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            var httpResponseMappers = options.Value.HttpResponseMappers.ToArray();
            httpResponseMappers.Should().HaveCount(2);
            httpResponseMappers[0].Should().BeOfType<TestProblemDetailsHttpResponseMapper>().Which.Status.Should().Be(418);
            httpResponseMappers[1].Should().BeOfType<ProblemDetailsHttpResponseMapper>().Which.Status.Should().Be(int.MinValue);
        }

        [Fact]
        public void Configure_Should_ConfigureHttpResponseMappers_InCorrectOrder()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o =>
            {
                o.HttpResponseMapper<TestProblemDetailsHttpResponseMapper>(500);
                o.HttpResponseMapper<ProblemDetailsHttpResponseMapper>(418);
                o.HttpResponseMapper<TestProblemDetailsHttpResponseMapper>();
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.HttpResponseMapperDescriptors.Should().HaveCount(3);

            var httpResponseMappers = options.Value.HttpResponseMappers.ToArray();
            httpResponseMappers.Should().HaveCount(3);
            httpResponseMappers[0].Should().BeOfType<TestProblemDetailsHttpResponseMapper>();
            httpResponseMappers[1].Should().BeOfType<ProblemDetailsHttpResponseMapper>();
            httpResponseMappers[2].Should().BeOfType<TestProblemDetailsHttpResponseMapper>();
        }

        [Fact]
        public void Configure_Should_ConfigureIncludeExceptionDetailsFunc()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o => o.IncludeExceptionDetails = (context) => context != null );

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.IncludeExceptionDetails(new DefaultHttpContext()).Should().Be(true);
            options.Value.IncludeExceptionDetails(null).Should().Be(false);
        }

        [Fact]
        public void Configure_Should_ConfigureDefaultIncludeExceptionDetailsFunc()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions();

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            var context = new DefaultHttpContext();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(p => p.GetService(It.IsAny<Type>()));
            context.RequestServices = serviceProviderMock.Object;

            Action action = () => options.Value.IncludeExceptionDetails(context);

            action.Should().Throw<InvalidOperationException>();
            serviceProviderMock.Verify(p => p.GetService(It.IsAny<Type>()), Times.Once());
        }

        [Fact]
        public void Configure_Should_ConfigureIsExceptionResponseFunc()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o => o.IsExceptionResponse = (context) => context != null);

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.IsExceptionResponse(new DefaultHttpContext()).Should().Be(true);
            options.Value.IsExceptionResponse(null).Should().Be(false);
        }

        [Fact]
        public void Configure_Should_ConfigureDefaultIsExceptionResponseFunc()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions();

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            var context = new DefaultHttpContext();
            for (int i = 0; i < 999; i++)
            {
                context.Response.StatusCode = i;
                options.Value.IsExceptionResponse(context).Should().Be(i >= 400 && i < 600);
            }
        }
    }
}
