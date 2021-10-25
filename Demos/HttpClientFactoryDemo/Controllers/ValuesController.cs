using HttpClientFactoryDemo.Clients;
using HttpClientFactoryDemo.Clients.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HttpClientFactoryDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ITestServiceClient _testServiceClient;
        private readonly INamedServiceClient _namedServiceClient;
        private readonly TypedServiceClient _typedServiceClient;

        public ValuesController(TestServiceClient testServiceClient, NamedServiceClient namedServiceClient, TypedServiceClient typedServiceClient)
        {
            _testServiceClient = testServiceClient;
            _namedServiceClient = namedServiceClient;
            _typedServiceClient = typedServiceClient;
        }

        [HttpGet]
        public async Task<string> Test()
        {
            return await _typedServiceClient.GetAsync();
        }
    }

}
