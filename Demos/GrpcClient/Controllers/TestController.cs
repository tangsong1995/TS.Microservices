using GrpcServices;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static GrpcServices.OrderGrpc;

namespace GrpcClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly OrderGrpcClient _orderGrpcClient;

        public TestController(OrderGrpcClient orderGrpcClient)
        {
            _orderGrpcClient = orderGrpcClient;
        }

        [HttpGet]
        public async Task<CreateOrderResult> Test()
        {
            var result = await _orderGrpcClient.CreateOrderAsync(new GrpcServices.CreateOrderCommand() { BuyerId = "test" });
            return result;
        }
    }
}
