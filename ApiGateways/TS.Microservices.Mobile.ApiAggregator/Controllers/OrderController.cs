using Microsoft.AspNetCore.Mvc;

namespace TS.Microservices.Mobile.ApiAggregator.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        TS.Microservices.Ordering.API.Grpc.OrderService.OrderServiceClient _client;
        public OrderController(TS.Microservices.Ordering.API.Grpc.OrderService.OrderServiceClient client)
        {
            _client = client;

        }

        [HttpGet]
        public ActionResult GetOrders([FromQuery] Ordering.API.Grpc.SearchRequest request)
        {
            //添加其它服务的调用;
            var data = _client.Search(request);
            return Ok(data.Orders);
        }
    }
}
