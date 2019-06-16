using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
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
        private readonly IActionResultExecutor<ObjectResult> _executor;
        private readonly ILogger<HttpExceptionsMiddleware> _logger;

        private static readonly ActionDescriptor _emptyActionDescriptor = new ActionDescriptor();
        private static readonly RouteData _emptyRouteData = new RouteData();

        /// <summary>
        /// Initializes the HttpExceptionsMiddleware.
        /// </summary>
        public HttpExceptionsMiddleware(
            RequestDelegate next,
            IOptions<HttpExceptionsOptions> options,
            IActionResultExecutor<ObjectResult> executor,
            ILogger<HttpExceptionsMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Invokes the HttpExceptionsMiddleware.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

                if (!_options.Value.IsExceptionResponse(context))
                    return;

                if (context.Response.HasStarted)
                {
                    _logger.LogError("The response has already started, the HttpExceptions middleware will not be executed.");
                    return;
                }

                await ExecuteProblemDetailsResultAsync(context, CreateProblemDetailsResult(context));
            }
            catch (Exception ex)
            {
                if (context.Response.HasStarted)
                {
                    _logger.LogError(ex, "The response has already started, the HttpExceptions middleware will not be executed.");
                    throw; // rethrow the exception if we can't handle it properly
                }

                try
                {
                    // write the HttpException as ProblemDetails to the response
                    await ExecuteProblemDetailsResultAsync(context, CreateProblemDetailsResult(context, ex));
                    return;
                }
                catch (Exception ex2)
                {
                    _logger.LogError(ex2, "An exception was thrown attempting to execute the HttpExceptions middleware.");
                }

                throw; // rethrow the exception if we can't handle it properly.
            }
        }

        private ProblemDetailsResult CreateProblemDetailsResult(HttpContext context, Exception ex = null)
        {
            ProblemDetails problemDetails = null;
            if (ex != null)
            {
                var includeExceptionDetails = _options.Value.IncludeExceptionDetails(context);
                problemDetails = ex.ToProblemDetails(context.Request.Path, includeExceptionDetails);
            }

            //TODO: create ProblemDetails when the exception is null

            return new ProblemDetailsResult(problemDetails);
        }

        private Task ExecuteProblemDetailsResultAsync(HttpContext context, ProblemDetailsResult result)
        {
            context.Response.Clear();
            // Make sure problem responses are never cached.
            context.Response.Headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            context.Response.Headers.Append(HeaderNames.Pragma, "no-cache");
            context.Response.Headers.Append(HeaderNames.Expires, "0");
            context.Response.StatusCode = result.StatusCode.Value;

            var routeData = context.GetRouteData() ?? _emptyRouteData;
            var actionContext = new ActionContext(context, routeData, _emptyActionDescriptor);

            return _executor.ExecuteAsync(actionContext, result);
        }
    }
}
