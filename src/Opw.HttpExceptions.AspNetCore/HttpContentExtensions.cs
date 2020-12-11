using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions.AspNetCore.Serialization;

namespace Opw.HttpExceptions.AspNetCore
{
    /// <summary>
    /// Provides extension methods for HttpContent.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Serialize the HTTP content to ProblemDetails as an asynchronous operation.
        /// </summary>
        /// <param name="content">The HTTP content.</param>
        public static ProblemDetails ReadAsProblemDetails(this HttpContent content)
        {
            _ = content ?? throw new ArgumentNullException(nameof(content));

            if (!content.Headers.ContentType.MediaType.Equals("application/problem+json", StringComparison.OrdinalIgnoreCase))
            {
                throw new SerializationException("HttpContent is not of type \"application/problem+json\".");
            }

            var str = content.ReadAsStringAsync().Result;
            var converter = new ProblemDetailsJsonConverter();
            var reader = new Utf8JsonReader(Encoding.UTF8.GetBytes(str));
            var problemDetails = converter.Read(ref reader, typeof(ProblemDetails), new JsonSerializerOptions());

            return problemDetails;
        }
    }
}
