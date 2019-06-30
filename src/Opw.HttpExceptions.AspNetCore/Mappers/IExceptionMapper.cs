using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;

namespace Opw.HttpExceptions.AspNetCore.Mappers
{
    /// <summary>
    /// Interface for ExceptionMappers.
    /// </summary>
    public interface IExceptionMapper
    {
        /// <summary>
        /// Can the given type be mapped.
        /// </summary>
        /// <param name="type">The type to map.</param>
        bool CanMap(Type type);

        /// <summary>
        /// Maps the exception.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The HTTP context.</param>
        /// <param name="actionResult">A representation of the exception as a IActionResult.</param>
        bool TryMap(Exception exception, HttpContext context, out IStatusCodeActionResult actionResult);

        /// <summary>
        /// Creates and returns a representation of the exception as a IActionResult.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A representation of the exception as a IActionResult.</returns>
        IStatusCodeActionResult Map(Exception exception, HttpContext context);
    }
}