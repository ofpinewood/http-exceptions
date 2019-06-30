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
#if NETCOREAPP2_2
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
#endif
#if NETCOREAPP3_0
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
#endif
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddHttpExceptions();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpExceptions();
#if NETCOREAPP2_2
            app.UseAuthentication();
            app.UseMvc();
#endif
#if NETCOREAPP3_0
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
#endif
        }
    }
}
