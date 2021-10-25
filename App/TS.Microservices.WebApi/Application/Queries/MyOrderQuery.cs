using MediatR;
using System.Collections.Generic;

namespace TS.Microservices.WebApi.Application.Queries
{
    public class MyOrderQuery : IRequest<List<string>>
    {
        public string UserName { get; set; }
    }
}
