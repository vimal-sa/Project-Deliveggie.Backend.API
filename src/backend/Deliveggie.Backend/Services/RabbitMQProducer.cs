using EasyNetQ;

namespace Deliveggie.Backend.Services
{
    public class RabbitMQProducer<TRequest, TResponse>
    {
        public TResponse Send(TRequest request)
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                return bus.Rpc.Request<TRequest, TResponse>(request);
            }
        }
    }
}
