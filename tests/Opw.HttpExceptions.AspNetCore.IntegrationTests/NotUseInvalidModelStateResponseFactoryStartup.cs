#if NETCOREAPP2_2
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.HttpExceptions.AspNetCore
{

    public class NotUseInvalidModelStateResponseFactoryStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            IMvcBuilder mvcBuilder = null;
            mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            mvcBuilder.AddHttpExceptions(options => options.SuppressInvalidModelStateResponseFactoryOverride = true);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpExceptions();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
#endif