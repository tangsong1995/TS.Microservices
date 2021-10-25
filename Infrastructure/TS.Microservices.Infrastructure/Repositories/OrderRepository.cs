using TS.Microservices.Domain.Aggregate;
using TS.Microservices.Infrastructure.Core;

namespace TS.Microservices.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order, long, OrderingContext>, IOrderRepository
    {
        public OrderRepository(OrderingContext context) : base(context)
        {
        }
    }
}
