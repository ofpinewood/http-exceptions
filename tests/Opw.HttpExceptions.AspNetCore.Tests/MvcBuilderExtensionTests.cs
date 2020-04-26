using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class MvcBuilderExtensionTests
    {
        [Fact]
        public async Task AddHttpExceptions_Should_AddAllRequiredServices_UsingIMvcBuilder()
        {
            var services = new ServiceCollection();
            var mvcBuilder = new Mock<IMvcBuilder>();
            mvcBuilder.Setup(m => m.Services).Returns(services);

            mvcBuilder.Object.AddHttpExceptions(options => { });

            var servicesProvider = services.BuildServiceProvider();

            var middleware = new HttpExceptionsMiddleware(
                new Mock<RequestDelegate>().Object,
                servicesProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>(),
                new Mock<ILogger<HttpExceptionsMiddleware>>().Object);

            await middleware.Invoke(new DefaultHttpContext());
        }

        [Fact]
        public async Task AddHttpExceptions_Should_AddAllRequiredServices_UsingIMvcCoreBuilder()
        {
            var services = new ServiceCollection();
            var mvcBuilder = new Mock<IMvcCoreBuilder>();
            mvcBuilder.Setup(m => m.Services).Returns(services);

            mvcBuilder.Object.AddHttpExceptions(options => { });

            var servicesProvider = services.BuildServiceProvider();

            var middleware = new HttpExceptionsMiddleware(
                new Mock<RequestDelegate>().Object,
                servicesProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>(),
                new Mock<ILogger<HttpExceptionsMiddleware>>().Object);

            await middleware.Invoke(new DefaultHttpContext());
        }
    }
}
