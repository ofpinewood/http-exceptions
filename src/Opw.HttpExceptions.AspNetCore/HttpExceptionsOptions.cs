using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Represents the possible HttpExceptions options for services.
    /// </summary>
    public class HttpExceptionsOptions
    {
        //private readonly List<ProblemDetailsMapper> _mappers;

        /// <summary>
        /// Include exception details, default behavior is only to include exception details in a development environment.
        /// </summary>
        public Func<HttpContext, bool> IncludeExceptionDetails { get; set; }

        /// <summary>
        /// Is the response an exception and should it be handled by the HttpExceptions middleware.
        /// </summary>
        public Func<HttpContext, bool> IsExceptionResponse { get; set; }

        /// <summary>
        /// Initializes the HttpExceptionsOptions.
        /// </summary>
        public HttpExceptionsOptions()
        {
            //_mappers = new List<ProblemDetailsMapper>();
        }

        //public void Map<TException>(Func<TException, ProblemDetails> mapping) where TException : Exception
        //{
        //    Map<TException>((context, ex) => mapping(ex));
        //}

        //public void Map<TException>(Func<HttpContext, TException, ProblemDetails> mapping) where TException : Exception
        //{
        //    _mappers.Add(new ProblemDetailsMapper(typeof(TException), (context, ex) => mapping(context, (TException)ex)));
        //}

        //internal bool TryMapProblemDetails(HttpContext context, Exception exception, out ProblemDetails problem)
        //{
        //    foreach (var mapper in _mappers)
        //    {
        //        if (mapper.TryMap(context, exception, out problem))
        //        {
        //            return true;
        //        }
        //    }

        //    problem = default;
        //    return false;
        //}
    }

    //internal sealed class ProblemDetailsMapper
    //{
    //    private readonly Type _type;
    //    private readonly Func<HttpContext, Exception, ProblemDetails> _mapping;

    //    public ProblemDetailsMapper(Type type, Func<HttpContext, Exception, ProblemDetails> mapping)
    //    {
    //        _type = type;
    //        _mapping = mapping;
    //    }

    //    public bool CanMap(Type type)
    //    {
    //        return _type.IsAssignableFrom(type);
    //    }

    //    public bool TryMap(HttpContext context, Exception exception, out ProblemDetails problemDetails)
    //    {
    //        if (CanMap(exception.GetType()))
    //        {
    //            try
    //            {
    //                problemDetails = _mapping(context, exception);
    //                return true;
    //            }
    //            catch
    //            {
    //                problemDetails = default;
    //                return false;
    //            }
    //        }

    //        problemDetails = default;
    //        return false;
    //    }
    //}
}
