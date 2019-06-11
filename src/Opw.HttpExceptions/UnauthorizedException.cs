using System.Net;

namespace Opw.HttpExceptions
{
    public class UnauthorizedException : HttpException
    {
        public UnauthorizedException()
            : base(HttpStatusCode.Unauthorized, "Unauthorized.")
        {
        }

        public UnauthorizedException(string message)
            : base(HttpStatusCode.Unauthorized, message)
        {
        }
    }
}
