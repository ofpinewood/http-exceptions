using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if NETCOREAPP3_0
using Microsoft.Extensions.Hosting;
#endif

namespace Opw.HttpExceptions.AspNetCore.Sample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if NETCOREAPP2_2
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
#endif
#if NETCOREAPP3_0
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
#endif
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            services.AddHttpExceptions(options =>
            {
#if NETCOREAPP2_2
                // This is the same as the default behavior; only include exception details in a development environment.
                options.IncludeExceptionDetails = context => context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
#endif
#if NETCOREAPP3_0
                // This is the same as the default behavior; only include exception details in a development environment.
                options.IncludeExceptionDetails = context => context.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName == Environments.Development;
#endif
                // This is a simplified version of the default behavior; only include exception details for 4xx and 5xx responses.
                options.IsExceptionResponse = context => (context.Response.StatusCode >= 400 && context.Response.StatusCode < 600);
            });
        }

#if NETCOREAPP2_2
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
#endif
#if NETCOREAPP3_0
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#endif
        {
            // UseHttpExceptions is the first middleware component added to the pipeline. Therefore,
            // the UseHttpExceptions Middleware catches any exceptions that occur in later calls.
            // When using HttpExceptions you don't need to use UseExceptionHandler or UseDeveloperExceptionPage.
            app.UseHttpExceptions();

            if (!env.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

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
