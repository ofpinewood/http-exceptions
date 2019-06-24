using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Opw.HttpExceptions.AspNetCore
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var str = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static HttpContent ToHttpContent(this object obj)
        {
            var str = JsonConvert.SerializeObject(obj);
            return new StringContent(str);
        }
    }
}
