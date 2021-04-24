namespace Deliveggie.Backend.Services
{
    interface IRabbitMQProducer<TRequest, TResponse>
    {
        TResponse Send(TRequest request);
    }
}
