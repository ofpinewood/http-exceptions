using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net.Http;

namespace Opw.HttpExceptions.AspNetCore._Test
{
    public class TestWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public IConfigurationRoot Configuration { get; }

        public TestWebApplicationFactory()
        {
            Configuration = new ConfigurationBuilder().Build();
        }

        protected override IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
                .UseConfiguration(Configuration)
                .UseStartup<TStartup>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseSolutionRelativeContentRoot("tests/Opw.HttpExceptions.AspNetCore.IntegrationTests");
            base.ConfigureWebHost(builder);
        }

        public new HttpClient CreateClient()
        {
            var baseAddress = new Uri($"http://localhost/api/");
            return base.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = baseAddress });
        }
    }
}
