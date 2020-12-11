using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opw.HttpExceptions.AspNetCore.Serialization;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var str = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(str);
        }

        public static StringContent ToJsonContent(this object obj)
        {
            var str = JsonSerializer.Serialize(obj);
            return new StringContent(str, Encoding.UTF8, "application/json");
        }

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
