using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Hosting;
using System.Linq;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpExceptionsOptionsSetupTests
    {
        [Fact]
        public void Configure_Should_ConfigureExceptionMappers()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(o =>
            {
                o.ExceptionMapper<HttpException, ExceptionMapper<HttpException>>();
                o.ExceptionMapper<Exception, ExceptionMapper<Exception>>();
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
                o.ExceptionMapper<HttpException, ExceptionMapper<HttpException>>();
                o.ExceptionMapper<ArgumentException, ExceptionMapper<ArgumentException>>();
                o.ExceptionMapper<Exception, TestExceptionMapper>();
            });

            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>();

            options.Value.ExceptionMapperDescriptors.Should().HaveCount(3);

            var exceptionMappers = options.Value.ExceptionMappers.ToArray();
            exceptionMappers.Should().HaveCount(3);
            exceptionMappers[0].Should().BeOfType<ExceptionMapper<HttpException>>();
            exceptionMappers[1].Should().BeOfType<ExceptionMapper<ArgumentException>>();
            exceptionMappers[2].Should().BeOfType<TestExceptionMapper>();
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
