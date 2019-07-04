using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class TestHelper
    {
        public static void SetHostEnvironmentName(IWebHost webHost, string environmentName)
        {
#if NETCOREAPP2_2
            webHost.Services.GetRequiredService<IHostingEnvironment>().EnvironmentName = environmentName;
#endif
#if NETCOREAPP3_0
            webHost.Services.GetRequiredService<IWebHostEnvironment>().EnvironmentName = environmentName;
#endif
        }

        public static Mock<IOptions<HttpExceptionsOptions>> CreateHttpExceptionsOptionsMock(bool includeExceptionInfo)
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(options => options.IncludeExceptionInfo = (_) => includeExceptionInfo);
            
            var serviceProvider = services.BuildServiceProvider();

            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>().Value);

            return optionsMock;
        }

        public static ProblemDetailsExceptionMapper<TException> CreateProblemDetailsExceptionMapper<TException>(bool includeExceptionInfo)
            where TException : Exception
        {
            var optionsMock = CreateHttpExceptionsOptionsMock(includeExceptionInfo);

            return new ProblemDetailsExceptionMapper<TException>(optionsMock.Object);
        }

        public static ProblemDetailsHttpResponseMapper CreateProblemDetailsHttpResponseMapper(bool includeExceptionInfo)
        {
            var optionsMock = CreateHttpExceptionsOptionsMock(includeExceptionInfo);

            return new ProblemDetailsHttpResponseMapper(optionsMock.Object);
        }
    }
}
