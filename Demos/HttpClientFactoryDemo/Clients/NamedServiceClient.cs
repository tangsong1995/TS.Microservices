using HttpClientFactoryDemo.Clients.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientFactoryDemo.Clients
{
    public class NamedServiceClient: INamedServiceClient
    {
        IHttpClientFactory _httpClientFactory;

        const string _clientName = "NamedServiceClient";  //定义客户端名称

        public NamedServiceClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public async Task<string> GetAsync()
        {
            var client = _httpClientFactory.CreateClient(_clientName); //使用客户端名称获取客户端

            //使用client发起HTTP请求,这里使用相对路径来访问
            return await client.GetStringAsync("/WeatherForecast");
        }
    }
}
