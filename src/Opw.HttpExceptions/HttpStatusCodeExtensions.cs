using System.Net;
using System.Reflection;

namespace Opw.HttpExceptions
{
    internal static class HttpStatusCodeExtensions
    {
        internal static bool TryGetLink(this HttpStatusCode statusCode, out string link)
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
