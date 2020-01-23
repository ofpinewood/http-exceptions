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
            var mvcBuilder = services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            mvcBuilder.AddHttpExceptions();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpExceptions();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
