using System.Net;

namespace Opw.HttpExceptions
{
    public class UnsupportedMediaTypeException : HttpException
    {
        public UnsupportedMediaTypeException()
            : base(HttpStatusCode.UnsupportedMediaType, "Unsupported Media Type.")
        {
        }

        public UnsupportedMediaTypeException(string message)
            : base(HttpStatusCode.UnsupportedMediaType, message)
        {
        }
    }
}
