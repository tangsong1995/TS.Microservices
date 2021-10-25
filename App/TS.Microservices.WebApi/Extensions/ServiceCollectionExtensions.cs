using DotNetCore.CAP.Messages;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Bulkhead;
using Polly.Caching;
using Polly.CircuitBreaker;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TS.Microservices.Domain.Aggregate;
using TS.Microservices.Infrastructure;
using TS.Microservices.Infrastructure.Repositories;
using TS.Microservices.WebApi.Application.IntegrationEvents;


namespace TS.Microservices.WebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddMediatRServices(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(OrderingContextTransactionBehavior<,>));
            return services.AddMediatR(typeof(Order).Assembly, typeof(Program).Assembly);
        }


        public static IServiceCollection AddDomainContext(this IServiceCollection services, Action<DbContextOptionsBuilder> optionsAction)
        {
            return services.AddDbContext<OrderingContext>(optionsAction);
        }

        public static IServiceCollection AddInMemoryDomainContext(this IServiceCollection services)
        {
            return services.AddDomainContext(builder => builder.UseInMemoryDatabase("domanContextDatabase"));
        }

        public static IServiceCollection AddMySqlDomainContext(this IServiceCollection services, string connectionString)
        {
            return services.AddDomainContext(builder =>
            {
                builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ISubscriberService, SubscriberService>();

            services.AddCap(options =>
            {
                options.UseEntityFramework<OrderingContext>();
                options.UseRabbitMQ(options =>
                {
                    configuration.GetSection("RabbitMQ").Bind(options);
                });
                options.FailedRetryCount = 5;
                options.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetService<ILogger<Startup>>();
                    logger.LogError($@"A message of type {failed.MessageType} failed after executing {options.FailedRetryCount} several times, 
                        requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                };
                //options.UseDashboard();
            });

            return services;
        }

        public static IHttpClientBuilder AddPolly(this IHttpClientBuilder builder)
        {
            builder.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1 * 2))); // p.RetryAsync() 表示重试一次，可指定重试次数
            return builder;
        }

        public static IServiceCollection AddServiceHttpClient(this IServiceCollection services, IConfiguration configuration)
        {
            //熔断
            services.AddHttpClient("httpclient").AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>().CircuitBreakerAsync(
                 handledEventsAllowedBeforeBreaking: 10,
                 durationOfBreak: TimeSpan.FromSeconds(10),
                 onBreak: (r, t) => { },
                 onReset: () => { },
                 onHalfOpen: () => { }
                 ));

            //熔断
            services.AddHttpClient("httpclient").AddPolicyHandler(Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
                failureThreshold: 0.8,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { }));

            //熔断策略
            var breakPolicy = Policy<HttpResponseMessage>.Handle<HttpRequestException>().AdvancedCircuitBreakerAsync(
                failureThreshold: 0.8,
                samplingDuration: TimeSpan.FromSeconds(10),
                minimumThroughput: 100,
                durationOfBreak: TimeSpan.FromSeconds(20),
                onBreak: (r, t) => { },
                onReset: () => { },
                onHalfOpen: () => { });

            //降级策略
            var message = new HttpResponseMessage()
            {
                Content = new StringContent("{}")
            };
            var fallback = Policy<HttpResponseMessage>.Handle<BrokenCircuitException>().FallbackAsync(message);

            //重试策略
            var retry = Policy<HttpResponseMessage>.Handle<Exception>().WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1));

            var fallbackBreak = Policy.WrapAsync(fallback, retry, breakPolicy);
            services.AddHttpClient("httpclient").AddPolicyHandler(fallbackBreak);

            //限流策略
            var bulk = Policy.BulkheadAsync<HttpResponseMessage>(
                maxParallelization: 30,
                maxQueuingActions: 20,
                onBulkheadRejectedAsync: contxt => Task.CompletedTask
                );

            var fallback2 = Policy<HttpResponseMessage>.Handle<BulkheadRejectedException>().FallbackAsync(new HttpResponseMessage()
            {
                Content = new StringContent("{}")
            });
            var fallbackbulk = Policy.WrapAsync(fallback2, bulk);
            services.AddHttpClient("httpclient").AddPolicyHandler(fallbackbulk);

            services.AddSingleton<IAsyncCacheProvider, MyAsyncCacheProvider>();
            var serviceProvider = services.BuildServiceProvider();
            var cachePolicy = Policy.CacheAsync<HttpResponseMessage>(serviceProvider.GetRequiredService<IAsyncCacheProvider>(), TimeSpan.FromSeconds(10));
            services.AddHttpClient("httpclient").AddPolicyHandler(cachePolicy);

            return services;
        }
    }

    public class MyAsyncCacheProvider : IAsyncCacheProvider
    {
        public Task PutAsync(string key, object value, Ttl ttl, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            throw new NotImplementedException();
        }

        public Task<(bool, object)> TryGetAsync(string key, CancellationToken cancellationToken, bool continueOnCapturedContext)
        {
            throw new NotImplementedException();
        }
    }
}
