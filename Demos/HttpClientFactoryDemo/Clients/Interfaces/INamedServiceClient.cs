using System.Threading.Tasks;
using TS.Microservices.DependencyInjection.IServices;

namespace HttpClientFactoryDemo.Clients.Interfaces
{
    public interface INamedServiceClient : IScopedService
    {
        Task<string> GetAsync();
    }
}
