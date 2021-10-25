namespace TS.Microservices.WebApi.Application.IntegrationEvents
{
    public interface ISubscriberService
    {
        void OrderPaymentSucceeded(OrderPaymentSucceededIntegrationEvent @event);

        void OrderCreated(OrderCreatedIntegrationEvent @event);
    }
}
