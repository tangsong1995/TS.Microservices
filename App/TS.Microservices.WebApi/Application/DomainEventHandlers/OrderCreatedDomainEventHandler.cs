using DotNetCore.CAP;
using System.Threading;
using System.Threading.Tasks;
using TS.Microservices.Domain.Abstractions;
using TS.Microservices.Domain.Events;
using TS.Microservices.WebApi.Application.IntegrationEvents;

namespace TS.Microservices.WebApi.Application.DomainEventHandlers
{
    public class OrderCreatedDomainEventHandler : IDomainEventHandler<OrderCreatedDomainEvent>
    {
        ICapPublisher _capPublisher;
        public OrderCreatedDomainEventHandler(ICapPublisher capPublisher)
        {
            _capPublisher = capPublisher;
        }

        public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _capPublisher.PublishAsync("OrderCreated", new OrderCreatedIntegrationEvent(notification.Order.Id));
        }
    }
}
