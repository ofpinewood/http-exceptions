using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System;
using Microsoft.AspNetCore.Hosting;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class TestHelper
    {
        public static void SetHostEnvironmentName(IWebHost webHost, string environmentName)
        {
            _ = webHost ?? throw new ArgumentNullException(nameof(webHost));

            webHost.Services.GetRequiredService<IWebHostEnvironment>().EnvironmentName = environmentName;
        }

        public static Mock<IOptions<HttpExceptionsOptions>> CreateHttpExceptionsOptionsMock(bool includeExceptionDetails, Uri defaultHelpLink = null)
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(options =>
            {
                options.IncludeExceptionDetails = (_) => includeExceptionDetails;
                options.DefaultHelpLink = defaultHelpLink;
            });

            var serviceProvider = services.BuildServiceProvider();

            var optionsMock = new Mock<IOptions<HttpExceptionsOptions>>();
            optionsMock.Setup(o => o.Value).Returns(serviceProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>().Value);

            return optionsMock;
        }

        public static ProblemDetailsExceptionMapper<TException> CreateProblemDetailsExceptionMapper<TException>(bool includeExceptionDetails)
            where TException : Exception
        {
            var optionsMock = CreateHttpExceptionsOptionsMock(includeExceptionDetails);

            return new ProblemDetailsExceptionMapper<TException>(optionsMock.Object);
        }

        public static ProblemDetailsHttpResponseMapper CreateProblemDetailsHttpResponseMapper(bool includeExceptionDetails)
        {
            var optionsMock = CreateHttpExceptionsOptionsMock(includeExceptionDetails);

            return new ProblemDetailsHttpResponseMapper(optionsMock.Object);
        }
    }
}
