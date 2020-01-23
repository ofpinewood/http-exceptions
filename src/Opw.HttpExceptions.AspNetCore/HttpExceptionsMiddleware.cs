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
        private readonly ILogger<HttpExceptionsMiddleware> _logger;

        private static readonly ActionDescriptor _emptyActionDescriptor = new ActionDescriptor();
        private static readonly RouteData _emptyRouteData = new RouteData();

        /// <summary>
        /// Initializes the HttpExceptionsMiddleware.
        /// </summary>
        public HttpExceptionsMiddleware(
            RequestDelegate next,
            IOptions<HttpExceptionsOptions> options,
            ILogger<HttpExceptionsMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options ?? throw new ArgumentNullException(nameof(options));
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
                    LogError(null, "The response has already started, the HttpExceptions middleware will not be executed.");
                    return;
                }

                if (TryCreateActionResult(context, null, out var result))
                {
                    // TODO: should we also log this error response?
                    await ExecuteActionResultAsync(context, result);
                    return;
                }

                LogError(null, "The HttpExceptions middleware could not handle the exception.");
            }
            catch (Exception ex)
            {
                LogError(ex, "An unhandled exception has occurred while executing the request.");

                if (context.Response.HasStarted)
                {
                    LogError(ex, "The response has already started, the HttpExceptions middleware will not be executed.");
                    throw; // rethrow the exception if we can't handle it properly
                }

                try
                {
                    if (TryCreateActionResult(context, ex, out var actionResult))
                    {
                        await ExecuteActionResultAsync(context, actionResult);
                        return;
                    }
                }
                catch (Exception ex2)
                {
                    LogError(ex2, "An exception was thrown attempting to execute the HttpExceptions middleware.");
                }

                LogError(ex, "The HttpExceptions middleware could not handle the exception.");
                throw; // rethrow the exception if we can't handle it properly.
            }
        }

        private bool TryCreateActionResult(HttpContext context, Exception ex, out IStatusCodeActionResult actionResult)
        {
            if (ex != null && _options.Value.TryMap(ex, context, out actionResult))
                return true;

            if (_options.Value.TryMap(context.Response, out actionResult))
                return true;

            actionResult = default;
            return false;
        }

        private Task ExecuteActionResultAsync(HttpContext context, IStatusCodeActionResult actionResult)
        {
            context.Response.Clear();
            // Make sure problem responses are never cached.
            context.Response.Headers.Append(HeaderNames.CacheControl, "no-cache, no-store, must-revalidate");
            context.Response.Headers.Append(HeaderNames.Pragma, "no-cache");
            context.Response.Headers.Append(HeaderNames.Expires, "0");
            context.Response.StatusCode = actionResult.StatusCode.Value;

            var routeData = context.GetRouteData() ?? _emptyRouteData;
            var actionContext = new ActionContext(context, routeData, _emptyActionDescriptor);

            return actionResult.ExecuteResultAsync(actionContext);
        }

        private void LogError(Exception ex, string message)
        {
            if (!_options.Value.ShouldLogException(ex))
                return;

            if (ex != null)
                _logger.LogError(ex, message);
            else
                _logger.LogError(message);
        }
    }
}
