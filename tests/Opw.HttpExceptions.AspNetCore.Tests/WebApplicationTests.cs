using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class WebApplicationTests
    {
        [Fact]
        public void Test()
        {
            var factory = new WebApplicationFactory<StartupWithSuppressMapClientErrors>();
            var client = factory.CreateClient();
        }

        private class StartupWithSuppressMapClientErrors
        {
            //public TestStartup(IConfiguration configuration)
            //{
            //    Configuration = configuration;
            //}

            //public IConfiguration Configuration { get; }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                    .ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);

                services.AddHttpExceptions();
            }

            public void Configure(IApplicationBuilder app)
            {
                app.UseHttpExceptions();
                
                app.UseMvc();
            }
        }
    }
}
