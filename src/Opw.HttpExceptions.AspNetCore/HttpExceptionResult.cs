//using Microsoft.AspNetCore.Mvc;
//using Opw.HttpExceptions;
//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Text;

//namespace Opw.HttpExceptions.AspNetCore
//{
//    /// <summary>
//    /// Exception result based a implementation of problem details result for HTTP APIs
//    /// </summary>
//    /// <see cref="https://tools.ietf.org/html/rfc7807"/>
//    public class HttpExceptionResult : ObjectResult
//    {
//        protected HttpExceptionResult(ProblemDetails problemDetails) : base(problemDetails)
//        {
//            ContentTypes.Add("application/problem+json");
//            StatusCode = problemDetails.Status;
//        }

//        public static HttpExceptionResult Create(Exception exception, Uri requestPath, string result = null)
//        {
//            var response = new ExceptionResponse(exception, requestPath, result);
//            var problemDetails = new ProblemDetails
//            {
//                Status = GetStatusCode(exception),
//                Type = GetType(exception),
//                Title = GetTitle(exception),
//                Detail = GetDetail(exception),
//                Instance = HttpContext.Request.Path
//            };

//            return new HttpExceptionResult(response);
//        }


//    }
//}
