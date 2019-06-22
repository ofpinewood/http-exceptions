using Opw.HttpExceptions.AspNetCore.Mappers;
using System;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for HttpExceptionsOptions.
    /// </summary>
    public static class HttpExceptionsOptionsExtensions
    {
        /// <summary>
        /// Add an ExceptionMapper for the specified exception type.
        /// </summary>
        /// <typeparam name="TException">The exception type that is handled by the IExceptionMapper.</typeparam>
        /// <typeparam name="TExceptionMapper">A type that derives from IExceptionMapper.</typeparam>
        /// <param name="options">The HttpExceptionsOptions.</param>
        /// <param name="arguments">Optionally inject parameters through ExceptionMapper constructors.</param>
        public static void ExceptionMapper<TException, TExceptionMapper>(this HttpExceptionsOptions options, params object[] arguments)
            where TException : Exception
            where TExceptionMapper : IExceptionMapper
        {
            if (options.ExceptionMapperDescriptors.ContainsKey(typeof(TException)))
            {
                options.ExceptionMapperDescriptors[typeof(TException)] = new ExceptionMapperDescriptor {
                    Type = typeof(TExceptionMapper),
                    Arguments = arguments
                };
                return;
            }

            options.ExceptionMapperDescriptors.Add(typeof(TException), new ExceptionMapperDescriptor
            {
                Type = typeof(TExceptionMapper),
                Arguments = arguments
            });
        }

        /// <summary>
        /// Add an HttpResponseMapper for the specified status code. HttpResponseMappers handle unauthorized and other non-exceptions responses.
        /// </summary>
        /// <typeparam name="THttpResponseMapper">A type that derives from IHttpResponseMapper.</typeparam>
        /// <param name="options">The HttpExceptionsOptions.</param>
        /// <param name="status">The status code that is handled by the IHttpResponseMapper.</param>
        /// <param name="arguments">Optionally inject parameters through HttpResponseMapper constructors.</param>
        public static void HttpResponseMapper<THttpResponseMapper>(this HttpExceptionsOptions options, int status = int.MinValue, params object[] arguments)
            where THttpResponseMapper : IHttpResponseMapper
        {
            if (options.HttpResponseMapperDescriptors.ContainsKey(status))
            {
                options.HttpResponseMapperDescriptors[status] = new HttpResponseMapperDescriptor
                {
                    Type = typeof(THttpResponseMapper),
                    Arguments = arguments
                };
                return;
            }

            options.HttpResponseMapperDescriptors.Add(status, new HttpResponseMapperDescriptor
            {
                Type = typeof(THttpResponseMapper),
                Arguments = arguments
            });
        }
    }
}
