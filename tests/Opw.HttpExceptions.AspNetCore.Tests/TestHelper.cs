using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Opw.HttpExceptions.AspNetCore.Mappers;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
#if NETCOREAPP2_2
using System.Net.Http.Formatting;
#endif

namespace Opw.HttpExceptions.AspNetCore
{
    public static class TestHelper
    {
        public static IEnumerable<MediaTypeFormatter> ProblemDetailsMediaTypeFormatters { get; }

        static TestHelper()
        {
            var jsonMediaTypeFormatter = new JsonMediaTypeFormatter();
            jsonMediaTypeFormatter.SupportedMediaTypes.Clear();
            jsonMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/problem+json"));

            var xmlMediaTypeFormatter = new XmlMediaTypeFormatter();
            xmlMediaTypeFormatter.SupportedMediaTypes.Clear();
            xmlMediaTypeFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/problem+xml"));

            var mediaTypeFormatters = new List<MediaTypeFormatter>();
            mediaTypeFormatters.Add(jsonMediaTypeFormatter);
            mediaTypeFormatters.Add(xmlMediaTypeFormatter);
            ProblemDetailsMediaTypeFormatters = mediaTypeFormatters;
        }

        public static void SetHostEnvironmentName(IWebHost webHost, string environmentName)
        {
#if NETCOREAPP2_2
            webHost.Services.GetRequiredService<IHostingEnvironment>().EnvironmentName = environmentName;
#endif
#if NETCOREAPP3_0
            webHost.Services.GetRequiredService<IWebHostEnvironment>().EnvironmentName = environmentName;
#endif
        }

        public static Mock<IOptions<HttpExceptionsOptions>> CreateHttpExceptionsOptionsMock(bool includeExceptionDetails)
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(options => options.IncludeExceptionDetails = (_) => includeExceptionDetails);
            
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

        public static HttpResponseMapper CreateHttpResponseMapper(bool includeExceptionDetails)
        {
            var optionsMock = CreateHttpExceptionsOptionsMock(includeExceptionDetails);

            return new HttpResponseMapper(optionsMock.Object);
        }
    }
}
