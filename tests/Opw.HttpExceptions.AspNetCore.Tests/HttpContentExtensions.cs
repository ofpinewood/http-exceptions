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
            var str = await content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(str);
        }

        public static StringContent ToJsonContent(this object obj, string mediaType = "application/json")
        {
            var str = JsonSerializer.Serialize(obj);
            return new StringContent(str, Encoding.UTF8, mediaType);
        }
    }
}
