using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class ExceptionExtensions
    {
        public static ProblemDetails ToProblemDetails(this Exception ex, Uri requestPath)
        {
            var problemDetails = new ProblemDetails
            {
                Status = GetStatusCode(ex),
                Type = GetType(ex),
                Title = GetTitle(ex),
                Detail = GetDetail(ex),
                Instance = requestPath.AbsoluteUri
            };

            return problemDetails;
        }

        internal static int GetStatusCode(Exception ex)
        {
            if (ex is HttpException)
                return ((HttpException)ex).StatusCode;

            return (int)HttpStatusCode.InternalServerError;
        }

        internal static string GetTitle(Exception ex)
        {
            if (ex is HttpException)
                return ((HttpException)ex).Title;

            return "Error";
        }

        internal static string GetDetail(Exception ex)
        {
            return ex.Message;
        }

        internal static string GetType(Exception ex)
        {
            var errorType = "error";
            if (ex is HttpException)
                errorType = ((HttpException)ex).Title.ToSlug();

            return new Uri($"error:{errorType}").ToString();
        }
    }
}
