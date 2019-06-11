using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class ForbiddenException : HttpException
    {
        public ForbiddenException()
            : base(HttpStatusCode.Forbidden, "Forbidden.")
        {
        }

        public ForbiddenException(string message)
            : base(HttpStatusCode.Forbidden, message)
        {
        }

        public ForbiddenException(string message, Exception innerException)
            : base(HttpStatusCode.Forbidden, message, innerException)
        {
        }
    }
}
