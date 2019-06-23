using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// Maps the exception to ProblemDetails.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The HTTP context.</param>
        /// <param name="problemDetails">A ProblemDetails representation of the exception.</param>
        bool TryMap(Exception exception, HttpContext context, out ProblemDetails problemDetails);

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the exception.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A ProblemDetails representation of the exception.</returns>
        ProblemDetails Map(Exception exception, HttpContext context);
    }
}