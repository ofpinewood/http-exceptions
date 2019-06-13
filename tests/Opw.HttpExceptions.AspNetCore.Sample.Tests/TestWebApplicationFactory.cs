using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Opw.HttpExceptions.AspNetCore.Sample
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public IConfigurationRoot Configuration { get; }
        public IEnumerable<MediaTypeFormatter> ProblemDetailsMediaTypeFormatters { get; }

        public TestWebApplicationFactory()
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

            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            var builder = new WebHostBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<Startup>();
            return builder;
        }

        public new HttpClient CreateClient()
        {
            var baseAddress = new Uri($"http://localhost/api/");
            return base.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = baseAddress });
        }
    }
}
