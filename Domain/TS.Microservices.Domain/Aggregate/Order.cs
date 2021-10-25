using TS.Microservices.Domain.Abstractions;
using TS.Microservices.Domain.Events;

namespace TS.Microservices.Domain.Aggregate
{
    public class Order : Entity<long>, IAggregateRoot
    {
        public string UserId { get; private set; }

        public string UserName { get; private set; }

        public Address Address { get; private set; }

        public int ItemCount { get; private set; }

        protected Order()
        { }

        public Order(string userId, string userName, int itemCount, Address address)
        {
            this.UserId = userId;
            this.UserName = userName;
            this.Address = address;
            this.ItemCount = itemCount;

            //添加OrderCreatedDomainEvent领域事件
            this.AddDomainEvent(new OrderCreatedDomainEvent(this));
        }


        public void ChangeAddress(Address address)
        {
            this.Address = address;
        }

    }
}
