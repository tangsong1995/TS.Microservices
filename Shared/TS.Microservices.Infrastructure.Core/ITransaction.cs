using DotNetCore.CAP;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace TS.Microservices.Infrastructure.Core
{
    public interface ITransaction
    {
        IDbContextTransaction GetCurrentTransaction();

        bool HasActiveTransaction { get; }

        Task<IDbContextTransaction> BeginTransactionAsync(ICapPublisher capBus);

        Task CommitTransactionAsync(IDbContextTransaction transaction);

        void RollbackTransaction();
    }
}
