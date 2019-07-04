using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Opw.HttpExceptions.AspNetCore
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public async Task AddHttpExceptions_Should_AddAllRequiredServices()
        {
            var services = new ServiceCollection();
            services.AddHttpExceptions(options => options.IncludeExceptionInfo = (_) => true);

            var servicesProvider = services.BuildServiceProvider();

            var middleware = new HttpExceptionsMiddleware(
                new Mock<RequestDelegate>().Object,
                servicesProvider.GetRequiredService<IOptions<HttpExceptionsOptions>>(),
                new Mock<ILogger<HttpExceptionsMiddleware>>().Object);

            await middleware.Invoke(new DefaultHttpContext());
        }
    }
}
