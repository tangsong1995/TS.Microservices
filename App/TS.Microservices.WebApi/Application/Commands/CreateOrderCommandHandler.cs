using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TS.Microservices.Domain.Aggregate;
using TS.Microservices.Infrastructure.Repositories;

namespace TS.Microservices.WebApi.Application.Commands
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        IOrderRepository _orderRepository;
        public CreateOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }


        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var address = new Address("han guo zhong xin", "shenzhen", "000000");
            var order = new Order("ts", "tangsong", request.ItemCount, address);

            _orderRepository.Add(order);
            await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
            return order.Id;
        }
    }
}
