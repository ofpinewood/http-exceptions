using System;
using System.Net;

namespace Opw.HttpExceptions
{
    public class ServiceUnavailableException : HttpException
    {
        public ServiceUnavailableException()
          : base(HttpStatusCode.ServiceUnavailable, "Service Unavailable.")
        {
        }

        public ServiceUnavailableException(string message)
            : base(HttpStatusCode.ServiceUnavailable, message)
        {
        }

        public ServiceUnavailableException(string message, Exception innerException)
            : base(HttpStatusCode.ServiceUnavailable, message, innerException)
        {
        }
    }
}
