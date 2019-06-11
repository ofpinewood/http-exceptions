using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class NotFoundException : HttpException
    {
        public NotFoundException(string message)
            : base(HttpStatusCode.NotFound, message)
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(HttpStatusCode.NotFound, message, innerException)
        {
        }
    }
}
