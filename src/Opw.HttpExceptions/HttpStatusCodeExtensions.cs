using System.Net;
using System.Reflection;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Extension methods for HttpStatusCode.
    /// </summary>
    public static class HttpStatusCodeExtensions
    {
        /// <summary>
        /// Try get a status code information link (https://tools.ietf.org/html/rfc7231).
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="link">The status code information link.</param>
        public static bool TryGetInformationLink(this int statusCode, out string link)
        {
            try
            {
                return ((HttpStatusCode)statusCode).TryGetInformationLink(out link);
            }
            catch { }

            link = null;
            return false;
        }

        /// <summary>
        /// Try get a status code information link (https://tools.ietf.org/html/rfc7231).
        /// </summary>
        /// <param name="statusCode">HTTP status code.</param>
        /// <param name="link">The status code information link.</param>
        public static bool TryGetInformationLink(this HttpStatusCode statusCode, out string link)
        {
            try
            {
                var field = typeof(ResponseStatusCodeLink).GetField(statusCode.ToString(), BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                link = field.GetRawConstantValue().ToString();
                return true;
            }
            catch { }

            link = null;
            return false;
        }
    }
}
