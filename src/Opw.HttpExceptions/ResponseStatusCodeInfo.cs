using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

namespace Opw.HttpExceptions
{
    /// <summary>
    /// Status code information based on https://tools.ietf.org/html/rfc7231.
    /// </summary>
    public class ResponseStatusCodeInfo
    {
        /// <summary>
        /// Status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Link to the RFC7231 definition.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Initializes the ResponseStatusCodeInfoDictionary.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="link"></param>
        public ResponseStatusCodeInfo(HttpStatusCode statusCode, string link)
        {
            StatusCode = statusCode;
            Link = link;
        }
    }
}
