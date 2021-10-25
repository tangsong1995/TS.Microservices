using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TS.Microservices.WebApi.Application.Queries
{
    public class MyOrderQueryHandler : IRequestHandler<MyOrderQuery, List<string>>
    {
        public Task<List<string>> Handle(MyOrderQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new List<string>() { DateTime.Now.ToString() });
        }
    }
}
