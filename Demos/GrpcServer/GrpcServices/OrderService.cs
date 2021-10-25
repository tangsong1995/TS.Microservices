using Grpc.Core;
using GrpcServices;
using System.Threading.Tasks;

namespace GrpcServer.GrpcServices
{
    public class OrderService : OrderGrpc.OrderGrpcBase
    {
        public override Task<CreateOrderResult> CreateOrder(CreateOrderCommand request, ServerCallContext context)
        {
            // do sth. save order
            return Task.FromResult(new CreateOrderResult { OrderId = 1 });
        }
    }
}
