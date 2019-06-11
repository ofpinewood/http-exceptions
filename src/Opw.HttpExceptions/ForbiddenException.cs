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
    }
}
