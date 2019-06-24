using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.HttpExceptions.AspNetCore
{

    public class NotUseInvalidModelStateResponseFactoryStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddHttpExceptions(options => {
                options.SuppressInvalidModelStateResponseFactoryOverride = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpExceptions();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
            //app.UseMvc();
        }
    }
}
