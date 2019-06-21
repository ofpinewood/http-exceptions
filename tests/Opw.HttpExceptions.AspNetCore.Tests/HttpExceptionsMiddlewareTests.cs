using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class HttpExceptionsMiddlewareTests
    {
        [Fact]
        public async Task Get_Should_ReturnProblemDetails()
        {
            var builder = CreateWebHostBuilder(ResponseThrowsException());

            using (var server = new TestServer(builder))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(string.Empty);

                var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.InternalServerError, TestHelper.ProblemDetailsMediaTypeFormatters);
                problemDetails.Extensions.Should().HaveCount(0);
            }
        }

        [Fact]
        public async Task Get_Should_ReturnProblemDetails_WhenThrowApplicationException()
        {
            var builder = CreateWebHostBuilder(ResponseThrowsException(new ApplicationException("ApplicationException!")));

            using (var server = new TestServer(builder))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(string.Empty);

                var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.InternalServerError, TestHelper.ProblemDetailsMediaTypeFormatters);
                problemDetails.Extensions.Should().HaveCount(0);
                problemDetails.Title.Should().Be("Application");
                problemDetails.Type.Should().Be("error:application");
                problemDetails.Detail.Should().Be("ApplicationException!");
            }
        }

        [Fact]
        public async Task Get_Should_ReturnProblemDetails_WithExceptionDetails()
        {
            var builder = CreateWebHostBuilder(ResponseThrowsException(new HttpException("HttpException!", new ApplicationException())), environment: EnvironmentName.Development);

            using (var server = new TestServer(builder))
            using (var client = server.CreateClient())
            {
                var response = await client.GetAsync(string.Empty);

                var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.InternalServerError, TestHelper.ProblemDetailsMediaTypeFormatters);
                problemDetails.Extensions.Should().HaveCount(1);

                var exceptionDetails = problemDetails.ShouldHaveExceptionDetails();
                exceptionDetails.Name.Should().Be(nameof(HttpException));
                exceptionDetails.InnerException.Should().NotBeNull();
                exceptionDetails.InnerException.Name.Should().Be(nameof(ApplicationException));
            }
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.NotImplemented)]
        [InlineData(HttpStatusCode.ServiceUnavailable)]
        [InlineData(HttpStatusCode.InternalServerError)]
        public async Task ErrorStatusCode_IsHandled(HttpStatusCode expected)
        {
            //using (var server = CreateServer(handler: ResponseWithStatusCode(expected)))
            //using (var client = server.CreateClient())
            //{
            //    var response = await client.GetAsync(string.Empty);

            //    Assert.Equal(expected, response.StatusCode);
            //    await AssertIsProblemDetailsResponse(response);
            //}
        }

        [Theory]
        [InlineData(HttpStatusCode.OK)]
        [InlineData(HttpStatusCode.Created)]
        [InlineData(HttpStatusCode.NoContent)]
        [InlineData((HttpStatusCode)600)]
        [InlineData((HttpStatusCode)800)]
        public async Task SuccessStatusCode_IsNotHandled(HttpStatusCode expected)
        {
            //using (var server = CreateServer(handler: ResponseWithStatusCode(expected)))
            //using (var client = server.CreateClient())
            //{
            //    var response = await client.GetAsync(string.Empty);

            //    Assert.Equal(expected, response.StatusCode);
            //    Assert.Equal(0, response.Content.Headers.ContentLength);
            //}
        }

        /// <summary>
        /// This test is to check if we can handle the default ClientError behavior correctly,
        /// when ApiBehaviorOptions.SuppressMapClientErrors = true.
        /// </summary>
        [Fact]
        public async Task Post_Should_ReturnProblemDetails_WhenResponseIsOfTypeIClientErrorActionResult()
        {
            var builder = CreateWebHostBuilder();

            using (var server = new TestServer(builder))
            using (var client = server.CreateClient())
            {
                var response = await client.PostAsync("api/test", new ObjectContent(typeof(TestModel), new TestModel(), new JsonMediaTypeFormatter()));

                var problemDetails = response.ShouldBeProblemDetails(HttpStatusCode.BadRequest, TestHelper.ProblemDetailsMediaTypeFormatters);
                problemDetails.Extensions.Should().HaveCount(0);
            }
        }

        #region Private Helper Methods

        private RequestDelegate ResponseThrowsException(Exception ex = null)
        {
            return _ => throw ex ?? new HttpException("HttpException!");
        }

        ///// <summary>
        ///// This the result that is returned by the ProblemDetailsClientErrorFactory when ApiBehaviorOptions.SuppressMapClientErrors = true.
        ///// The ProblemDetailsClientErrorFactory returns a ObjectResult with problem details
        ///// </summary>
        ///// <remarks>
        ///// See: https://github.com/aspnet/AspNetCore/blob/c565386a3ed135560bc2e9017aa54a950b4e35dd/src/Mvc/Mvc.Core/src/Infrastructure/ProblemDetailsClientErrorFactory.cs
        ///// </remarks>
        //private RequestDelegate ResponseClientErrorActionResult()
        //{
        //    return context =>
        //    {
        //        var executor = context.RequestServices.GetService<IActionResultExecutor<ObjectResult>>();
        //        var actionContext = new ActionContext(context, new RouteData(), new ActionDescriptor());

        //        var problemDetails = new ProblemDetails { Status = (int)HttpStatusCode.BadRequest, Title = "BadRequest", Type = "about:blank" };
        //        var result = new ObjectResult(problemDetails)
        //        {
        //            StatusCode = problemDetails.Status,
        //            ContentTypes = { "application/problem+json", "application/problem+xml" },
        //        };

        //        return executor.ExecuteAsync(actionContext, result);
        //    };
        //}

        private IWebHostBuilder CreateWebHostBuilder(
            RequestDelegate handler = null,
            Action<IServiceCollection> configureServices = null,
            Action<IMvcCoreBuilder> configureMvcCore = null,
            string environment = null)
        {
            if (configureServices == null) configureServices = _ => { };
            if (configureMvcCore == null) configureMvcCore = _ => { };

            return new WebHostBuilder()
                .UseEnvironment(environment ?? EnvironmentName.Production)
                .ConfigureServices(services =>
                {
                    services
                        .AddHttpExceptions()
                        .AddCors();

                    configureMvcCore(services
                        .AddMvcCore()
                        .AddApplicationPart(GetType().Assembly)
                        .AddJsonFormatters());

                    configureServices(services);
                })
                .Configure(app =>
                {
                    app
                        .UseHttpExceptions()
                        .UseCors(y => y.AllowAnyOrigin())
                        .UseMvc();

                    if (handler != null) app.Run(handler);
                });
        }

        [Route("api/[controller]")]
        [ApiController]
        public class TestController : ControllerBase
        {
            [HttpPost]
            public ActionResult<IActionResult> Post(TestModel model)
            {
                return Ok(model);
            }
        }

        public class TestModel
        {
            [Required]
            public string Id { get; set; }

            public string Name { get; set; }

            public int Prince { get; set; }
        }

        #endregion
    }
}
