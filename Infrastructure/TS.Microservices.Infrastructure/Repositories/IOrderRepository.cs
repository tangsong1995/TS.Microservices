using TS.Microservices.Domain.Aggregate;
using TS.Microservices.Infrastructure.Core;

namespace TS.Microservices.Infrastructure.Repositories
{
    public interface IOrderRepository : IRepository<Order, long>
    {

    }
}
