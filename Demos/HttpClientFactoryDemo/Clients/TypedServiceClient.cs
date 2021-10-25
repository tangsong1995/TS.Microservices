using System.Net.Http;
using System.Threading.Tasks;
namespace HttpClientFactoryDemo.Clients
{
    public class TypedServiceClient
    {
        HttpClient _client;
        public TypedServiceClient(HttpClient client)
        {
            _client = client;
        }


        public async Task<string> GetAsync()
        {
           return  await _client.GetStringAsync("/WeatherForecast"); //这里使用相对路径来访问
        }
    }
}
