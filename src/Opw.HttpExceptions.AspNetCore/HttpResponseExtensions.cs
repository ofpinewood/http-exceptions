using Microsoft.AspNetCore.Http;
using System;

namespace Opw.HttpExceptions.AspNetCore
{
    internal static class HttpResponseExtensions
    {
        internal static bool IsProblemDetailsResponse(this HttpResponse response)
        {
            if (response?.ContentType?.IndexOf("application/problem+json", StringComparison.InvariantCultureIgnoreCase) >= 0)
                return true;
            if (response?.ContentType?.IndexOf("application/problem+xml", StringComparison.InvariantCultureIgnoreCase) >= 0)
                return true;
            return false;
        }
    }
}
