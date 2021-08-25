using System.Net.Http;
using System.Threading.Tasks;

namespace Moq.HttpClientUtilities.Samples
{
    public class MyService
    {
        private readonly HttpClient _httpClient;

        internal const string Path = "path";

        public MyService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetValue() => await _httpClient.GetStringAsync(Path);
    }
}