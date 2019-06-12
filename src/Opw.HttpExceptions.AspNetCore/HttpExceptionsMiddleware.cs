using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// HttpExceptions middleware.
    /// </summary>
    public class HttpExceptionsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<HttpExceptionsOptions> _options;
        //private readonly IActionResultExecutor<ObjectResult> _executor;
        //private readonly IHostingEnvironment _environment;
        private readonly ILogger<HttpExceptionsMiddleware> _logger;

        /// <summary>
        /// Initializes the HttpExceptionsMiddleware
        /// </summary>
        /// <param name="next"></param>
        /// <param name="options"></param>
        /// <param name="executor"></param>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public HttpExceptionsMiddleware(
            RequestDelegate next,
            IOptions<HttpExceptionsOptions> options,
            //IActionResultExecutor<ObjectResult> executor,
            //IHostingEnvironment environment,
            ILogger<HttpExceptionsMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));

            //_executor = executor;
            //_environment = environment;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the HttpExceptionsMiddleware
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            await _next(context);
        }
    }
}
