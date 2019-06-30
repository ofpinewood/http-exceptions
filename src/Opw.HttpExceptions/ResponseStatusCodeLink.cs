namespace Opw.HttpExceptions
{
    /// <summary>
    /// Status code information links to https://tools.ietf.org/html/rfc7231.
    /// </summary>
    public static class ResponseStatusCodeLink
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string BadRequest = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public const string Unauthorized = "https://tools.ietf.org/html/rfc7235#section-3.1";
        public const string PaymentRequired = "https://tools.ietf.org/html/rfc7231#section-6.5.2";
        public const string Forbidden = "https://tools.ietf.org/html/rfc7231#section-6.5.3";
        public const string NotFound = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
        public const string MethodNotAllowed = "https://tools.ietf.org/html/rfc7231#section-6.5.5";
        public const string NotAcceptable = "https://tools.ietf.org/html/rfc7231#section-6.5.6";
        public const string ProxyAuthenticationRequired = "https://tools.ietf.org/html/rfc7235#section-3.2";
        public const string RequestTimeout = "https://tools.ietf.org/html/rfc7231#section-6.5.7";
        public const string Conflict = "https://tools.ietf.org/html/rfc7231#section-6.5.8";
        public const string Gone = "https://tools.ietf.org/html/rfc7231#section-6.5.9";
        public const string LengthRequired = "https://tools.ietf.org/html/rfc7231#section-6.5.10";
        public const string PreconditionFailed = "https://tools.ietf.org/html/rfc7232#section-4.2";
        public const string RequestEntityTooLarge = "https://tools.ietf.org/html/rfc7231#section-6.5.11";
        public const string RequestUriTooLong = "https://tools.ietf.org/html/rfc7231#section-6.5.12";
        public const string UnsupportedMediaType = "https://tools.ietf.org/html/rfc7231#section-6.5.13";
        public const string RequestedRangeNotSatisfiable = "https://tools.ietf.org/html/rfc7233#section-4.4";
        public const string ExpectationFailed = "https://tools.ietf.org/html/rfc7231#section-6.5.14";
        public const string UpgradeRequired = "https://tools.ietf.org/html/rfc7231#section-6.5.15";
        public const string InternalServerError = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        public const string NotImplemented = "https://tools.ietf.org/html/rfc7231#section-6.6.2";
        public const string BadGateway = "https://tools.ietf.org/html/rfc7231#section-6.6.3";
        public const string ServiceUnavailable = "https://tools.ietf.org/html/rfc7231#section-6.6.4";
        public const string GatewayTimeout = "https://tools.ietf.org/html/rfc7231#section-6.6.5";
        public const string HttpVersionNotSupported = "https://tools.ietf.org/html/rfc7231#section-6.6.6";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
