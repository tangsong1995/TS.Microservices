using TS.Microservices.Domain.Abstractions;
using TS.Microservices.Domain.Aggregate;

namespace TS.Microservices.Domain.Events
{
    public class OrderCreatedDomainEvent : IDomainEvent
    {
        public Order Order { get; private set; }
        public OrderCreatedDomainEvent(Order order)
        {
            this.Order = order;
        }
    }
}
