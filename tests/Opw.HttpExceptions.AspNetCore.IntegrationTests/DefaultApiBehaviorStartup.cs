using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Opw.HttpExceptions.AspNetCore
{
    public class DefaultApiBehaviorStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddHttpExceptions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpExceptions();
            app.UseRouting();
            app.UseAuthentication();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
            //app.UseMvc();
        }
    }
}
