using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class TestsHelper
    {
        public static Mock<IOptions<HttpExceptionsOptions>> CreateHttpExceptionsOptionsMock(bool includeExceptionDetails)
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions((options) => options.IncludeExceptionDetails = (_) => includeExceptionDetails);
            
            var serviceProvider = services.BuildServiceProvider();

            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>().Value);

            return optionsMock;
        }

        public static ExceptionMapper<TException> CreateExceptionMapper<TException>(bool includeExceptionDetails)
            where TException : Exception
        {
            var optionsMock = CreateHttpExceptionsOptionsMock(includeExceptionDetails);

            return new ExceptionMapper<TException>(optionsMock.Object);
        }
    }
}
