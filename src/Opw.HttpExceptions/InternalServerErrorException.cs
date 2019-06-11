using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class InternalServerErrorException : HttpException
    {
        public InternalServerErrorException()
            : base(HttpStatusCode.InternalServerError, "Internal server error.")
        {
        }

        public InternalServerErrorException(Exception innerException)
            : base(HttpStatusCode.InternalServerError, "Internal server error.", innerException)
        {
        }

        public InternalServerErrorException(string message)
            : base(HttpStatusCode.InternalServerError, message)
        {
        }

        public InternalServerErrorException(string message, Exception innerException)
            : base(HttpStatusCode.InternalServerError, message, innerException)
        {
        }
    }
}
