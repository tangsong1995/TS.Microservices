using MediatR;

namespace TS.Microservices.WebApi.Application.Commands
{
    public class CreateOrderCommand : IRequest<long>
    {

        public CreateOrderCommand(int itemCount)
        {
            ItemCount = itemCount;
        }

        public int ItemCount { get; private set; }
    }
}
