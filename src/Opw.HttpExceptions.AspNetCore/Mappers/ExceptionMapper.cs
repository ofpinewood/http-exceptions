using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    /// <summary>
    /// Default mapper for mapping Exceptions to ProblemDetails.
    /// </summary>
    public class ExceptionMapper<TException> : IExceptionMapper where TException : Exception
    {
        /// <summary>
        /// HttpExceptions options.
        /// </summary>
        protected IOptions<HttpExceptionsOptions> Options { get; }

        /// <summary>
        /// Initializes the ExceptionMapper.
        /// </summary>
        public ExceptionMapper(IOptions<HttpExceptionsOptions> options)
        {
            Options = options;
        }

        /// <summary>
        /// Can the given type be mapped.
        /// </summary>
        /// <param name="type">The type to map.</param>
        public virtual bool CanMap(Type type)
        {
            return typeof(TException).IsAssignableFrom(type);
        }

        /// <summary>
        /// Maps the exception to ProblemDetails.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="problemDetails">A ProblemDetails representation of the exception.</param>
        public virtual bool TryMap(Exception exception, HttpContext context, out ProblemDetails problemDetails)
        {
            problemDetails = default;

            if (!CanMap(exception.GetType()))
                return false;

            try
            {
                problemDetails = Map(exception, context);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the exception.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A ProblemDetails representation of the exception.</returns>
        public virtual ProblemDetails Map(Exception exception, HttpContext context)
        {
            if (!(exception is TException ex))
                throw new ArgumentOutOfRangeException(nameof(exception), exception, $"Exception is not of type {typeof(TException).Name}.");

            var problemDetails = new ProblemDetails
            {
                Status = MapStatus(ex, context),
                Type = MapType(ex, context),
                Title = MapTitle(ex, context),
                Detail = MapDetail(ex, context),
                Instance = MapInstance(ex, context)
            };

            if (Options.Value.IncludeExceptionDetails(context))
                problemDetails.Extensions.Add(nameof(ExceptionDetails).ToCamelCase(), new ExceptionDetails(exception));

            return problemDetails;
        }

        /// <summary>
        /// Map the ProblemDetails.Instance property using the exception and/or the HTTP context.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>Returns either the request path, the exception help link or null.</returns>
        protected virtual string MapInstance(TException exception, HttpContext context)
        {
            if (context.Request != null && context.Request.Path.HasValue)
                return context.Request.Path;

            var link = exception.HelpLink;
            if (string.IsNullOrWhiteSpace(link))
                return null;

            if (Uri.TryCreate(link, UriKind.Absolute, out var result))
                return result.ToString();

            return null;
        }

        /// <summary>
        /// Map the ProblemDetails.Status property using the exception and/or the HTTP context.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>Returns the HttpException status code or 500 (InternalServerError) for other exception types.</returns>
        protected virtual int MapStatus(TException exception, HttpContext context)
        {
            if (exception is HttpExceptionBase httpException)
                return (int)httpException.StatusCode;

            return (int)HttpStatusCode.InternalServerError;
        }

        /// <summary>
        /// Map the ProblemDetails.Title property using the exception and/or the HTTP context.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>Returns the Exception type name without the "Exception" suffix.</returns>
        protected virtual string MapTitle(TException exception, HttpContext context)
        {
            var name = exception.GetType().Name;
            if (name.Contains("`")) name = name.Substring(0, name.IndexOf('`'));
            if (name.EndsWith("Exception", StringComparison.InvariantCultureIgnoreCase))
                name = name.Substring(0, name.Length - "Exception".Length);

            return name;
        }

        /// <summary>
        /// Map the ProblemDetails.Detail property using the exception and/or the HTTP context.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>Returns the Exception message.</returns>
        protected virtual string MapDetail(TException exception, HttpContext context)
        {
            return exception.Message;
        }

        /// <summary>
        /// Map the ProblemDetails.Type property using the exception and/or the HTTP context.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>Returns the URI with the Exception type name ("error:[Type:slug]").</returns>
        protected virtual string MapType(TException exception, HttpContext context)
        {
            return new Uri($"error:{MapTitle(exception, context).ToSlug()}").ToString();
        }
    }
}
