using DotNetCore.CAP;
using Microsoft.Extensions.Logging;
using TS.Microservices.Infrastructure.Core.Behaviors;

namespace TS.Microservices.Infrastructure
{
    public class OrderingContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<OrderingContext, TRequest, TResponse>
    {
        public OrderingContextTransactionBehavior(OrderingContext dbContext, ILogger<OrderingContextTransactionBehavior<TRequest, TResponse>> logger, ICapPublisher capBus) : base(dbContext, logger,capBus)
        {
        }
    }
}
