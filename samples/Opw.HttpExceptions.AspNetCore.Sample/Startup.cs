using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpExceptions((options) =>
            {
                // This is the same as the default behavior; only include exception details in a development environment.
                options.IncludeExceptionDetails = context => context.RequestServices.GetRequiredService<IHostingEnvironment>().IsDevelopment();
                // This is a simplified version of the default behavior; only include exception details for 4xx and 5xx responses.
                options.IsExceptionResponse = context => (context.Response.StatusCode < 400 && context.Response.StatusCode >= 600);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseMvc();
        }
    }
}
