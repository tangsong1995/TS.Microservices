using HttpClientFactoryDemo.Clients.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactoryDemo.Clients
{
    public class TestServiceClient : ITestServiceClient
    {
        IHttpClientFactory _httpClientFactory;

        public TestServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetAsync()
        {
            var client = _httpClientFactory.CreateClient();

            //使用client发起HTTP请求
            return await client.GetStringAsync("https://localhost:5011/WeatherForecast");
        }
    }
}
