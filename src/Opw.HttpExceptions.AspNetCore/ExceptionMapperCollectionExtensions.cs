using System;
using System.Collections.Generic;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for HttpExceptionsOptions.
    /// </summary>
    public static class HttpExceptionsOptionsExtensions
    {
        /// <summary>
        /// Add ExceptionMappers.
        /// </summary>
        /// <typeparam name="TExceptionMapper">A type that derives from IExceptionMapper.</typeparam>
        /// <param name="options">The HttpExceptionsOptions.</param>
        /// <param name="arguments">Optionally inject parameters through ExceptionMapper constructors.</param>
        public static void ExceptionMapper<TExceptionMapper>(this HttpExceptionsOptions options, params object[] arguments)
             where TExceptionMapper : IExceptionMapper<Exception>
        {
            options.ExceptionMapperDescriptors.Add(new ExceptionMapperDescriptor
            {
                Type = typeof(TExceptionMapper),
                Arguments = arguments
            });
        }
    }
}
