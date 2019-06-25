using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Status code information based on https://tools.ietf.org/html/rfc7231.
    /// </summary>
    public class ResponseStatusCodeInfoDictionary : ReadOnlyDictionary<HttpStatusCode, ResponseStatusCodeInfo>
    {
        /// <summary>
        /// Initializes the ResponseStatusCodeInfoDictionary.
        /// </summary>
        public ResponseStatusCodeInfoDictionary() : base(CreateResponseStatusCodeInfoDictionary()) { }

        private static ReadOnlyDictionary<HttpStatusCode, ResponseStatusCodeInfo> CreateResponseStatusCodeInfoDictionary()
        {
            var dictionary = new Dictionary<HttpStatusCode, ResponseStatusCodeInfo>();

            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.BadRequest, "https://tools.ietf.org/html/rfc7231#section-6.5.1");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.Unauthorized, "https://tools.ietf.org/html/rfc7235#section-3.1");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.PaymentRequired, "https://tools.ietf.org/html/rfc7231#section-6.5.2");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.Forbidden, "https://tools.ietf.org/html/rfc7231#section-6.5.3");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.NotFound, "https://tools.ietf.org/html/rfc7231#section-6.5.4");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.MethodNotAllowed, "https://tools.ietf.org/html/rfc7231#section-6.5.5");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.NotAcceptable, "https://tools.ietf.org/html/rfc7231#section-6.5.6");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.ProxyAuthenticationRequired, "https://tools.ietf.org/html/rfc7235#section-3.2");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.RequestTimeout, "https://tools.ietf.org/html/rfc7231#section-6.5.7");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.Conflict, "https://tools.ietf.org/html/rfc7231#section-6.5.8");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.Gone, "https://tools.ietf.org/html/rfc7231#section-6.5.9");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.LengthRequired, "https://tools.ietf.org/html/rfc7231#section-6.5.10");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.PreconditionFailed, "https://tools.ietf.org/html/rfc7232#section-4.2");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.RequestEntityTooLarge, "https://tools.ietf.org/html/rfc7231#section-6.5.11");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.RequestUriTooLong, "https://tools.ietf.org/html/rfc7231#section-6.5.12");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.UnsupportedMediaType, "https://tools.ietf.org/html/rfc7231#section-6.5.13");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.RequestedRangeNotSatisfiable, "https://tools.ietf.org/html/rfc7233#section-4.4");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.ExpectationFailed, "https://tools.ietf.org/html/rfc7231#section-6.5.14");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.UpgradeRequired, "https://tools.ietf.org/html/rfc7231#section-6.5.15");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.InternalServerError, "https://tools.ietf.org/html/rfc7231#section-6.6.1");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.NotImplemented, "https://tools.ietf.org/html/rfc7231#section-6.6.2");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.BadGateway, "https://tools.ietf.org/html/rfc7231#section-6.6.3");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.ServiceUnavailable, "https://tools.ietf.org/html/rfc7231#section-6.6.4");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.GatewayTimeout, "https://tools.ietf.org/html/rfc7231#section-6.6.5");
            dictionary.AddResponseStatusCodeInfo(HttpStatusCode.HttpVersionNotSupported, "https://tools.ietf.org/html/rfc7231#section-6.6.6");

            return new ReadOnlyDictionary<HttpStatusCode, ResponseStatusCodeInfo>(dictionary);
        }
    }
}
