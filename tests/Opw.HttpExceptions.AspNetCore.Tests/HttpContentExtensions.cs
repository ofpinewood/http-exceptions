using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            _ = content ?? throw new ArgumentNullException(nameof(content));

            var str = await content.ReadAsStringAsync();
            // WARNING: Newtonsoft can only be used here because this is a test project
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

        public static StringContent ToJsonContent(this object obj, string mediaType = "application/json")
        {
            var str = JsonSerializer.Serialize(obj, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            return new StringContent(str, Encoding.UTF8, mediaType);
        }
    }
}
