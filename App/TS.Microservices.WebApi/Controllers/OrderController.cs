using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TS.Microservices.WebApi.Application.Commands;
using TS.Microservices.WebApi.Application.Queries;

namespace TS.Microservices.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<long> CreateOrder([FromBody] CreateOrderCommand cmd)
        {
            return await _mediator.Send(cmd, HttpContext.RequestAborted);
        }

        [HttpGet]
        public async Task<List<string>> QueryOrder([FromQuery] MyOrderQuery myOrderQuery)
        {
            return await _mediator.Send(myOrderQuery);
        }

    }
}
