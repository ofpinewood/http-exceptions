using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class BadRequestException : HttpException
    {
        public BadRequestException()
            : base(HttpStatusCode.BadRequest, "Bad request.")
        {
        }

        public BadRequestException(string message)
            : base(HttpStatusCode.BadRequest, message)
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(HttpStatusCode.BadRequest, message, innerException)
        {
        }
    }
}
