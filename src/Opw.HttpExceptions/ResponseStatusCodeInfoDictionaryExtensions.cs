using System.Collections.Generic;
using System.Net;

namespace Opw.HttpExceptions
{
    internal static class ResponseStatusCodeInfoDictionaryExtensions
    {
        internal static void AddResponseStatusCodeInfo(this IDictionary<HttpStatusCode, ResponseStatusCodeInfo> dictionary, HttpStatusCode statusCode, string link)
        {
            dictionary.Add(statusCode, new ResponseStatusCodeInfo(statusCode, link));
        }
    }
}
