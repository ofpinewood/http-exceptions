using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class ConflictException : HttpException
    {
        public ConflictException()
            : base(HttpStatusCode.Conflict, "Conflict.")
        {
        }

        public ConflictException(string message)
            : base(HttpStatusCode.Conflict, message)
        {
        }

        public ConflictException(string message, Exception innerException)
            : base(HttpStatusCode.Conflict, message, innerException)
        {
        }
    }
}
