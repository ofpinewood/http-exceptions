using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Opw.HttpExceptions.AspNetCore.Mappers;
using Opw.HttpExceptions.AspNetCore.Sample.CustomErrors;
using System;
using Microsoft.Extensions.Hosting;

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
            var mvcBuilder = services.AddControllers();
            mvcBuilder.AddHttpExceptions(options =>
            {
                // This is the same as the default behavior; only include exception details in a development environment.
                options.IncludeExceptionDetails = context => context.RequestServices.GetRequiredService<IWebHostEnvironment>().EnvironmentName == Environments.Development;
                // This is a simplified version of the default behavior; only map exceptions for 4xx and 5xx responses.
                options.IsExceptionResponse = context => (context.Response.StatusCode >= 400 && context.Response.StatusCode < 600);
                // Only log the when it has a status code of 500 or higher, or when it not is a HttpException.
                options.ShouldLogException = exception =>
                {
                    if ((exception is HttpExceptionBase httpException && (int)httpException.StatusCode >= 500) || !(exception is HttpExceptionBase))
                        return true;
                    return false;
                };

                // custom exception mapper does not map to Problem Details
                options.ExceptionMapper<FormatException, CustomExceptionMapper>();
                // default exception mapper for mapping to Problem Details
                options.ExceptionMapper<Exception, ProblemDetailsExceptionMapper<Exception>>();
            });

            // serializers for returning "application/xml"
            mvcBuilder.AddXmlSerializerFormatters().AddXmlDataContractSerializerFormatters();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }
    }
}
