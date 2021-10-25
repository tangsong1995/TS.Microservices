using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TS.Microservices.DependencyInjection.IServices;

namespace HttpClientFactoryDemo.DelegatingHandlers
{
    public class RequestIdDelegatingHandler : DelegatingHandler, ISingletonService
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //处理请求           
            request.Headers.Add("x-guid", Guid.NewGuid().ToString());

            var result = await base.SendAsync(request, cancellationToken); //调用内部handler

            //处理响应

            return result;
        }
    }
}
