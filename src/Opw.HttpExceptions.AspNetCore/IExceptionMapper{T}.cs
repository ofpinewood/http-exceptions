using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Interface for ExceptionMappers.
    /// </summary>
    public interface IExceptionMapper<TException> where TException : Exception
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
        /// <param name="context">The current HTTP context.</param>
        /// <param name="problemDetails">A ProblemDetails representation of the exception.</param>
        bool TryMap(TException exception, HttpContext context, out ProblemDetails problemDetails);

        /// <summary>
        /// Creates and returns a ProblemDetails representation of the exception.
        /// </summary>
        /// <param name="exception">The exception to map.</param>
        /// <param name="context">The HTTP context.</param>
        /// <returns>A ProblemDetails representation of the exception.</returns>
        ProblemDetails Map(TException exception, HttpContext context);

    }
}