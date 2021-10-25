using DotNetCore.CAP;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Microservices.Domain.Aggregate;
using TS.Microservices.Infrastructure.Core;
using TS.Microservices.Infrastructure.EntityConfigurations;

namespace TS.Microservices.Infrastructure
{
    public class OrderingContext : EFContext
    {
        public OrderingContext(DbContextOptions options, IMediator mediator) : base(options, mediator)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 注册领域模型与数据库的映射关系
            modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
            #endregion
            base.OnModelCreating(modelBuilder);
        }

    }
}
