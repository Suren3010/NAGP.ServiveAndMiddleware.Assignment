using RabbitMQ.Client;

namespace NAGP.Orders.Service.Services.Interfaces
{
    /// <summary>
    /// Contract for rabbitMq producer
    /// </summary>
    public interface IRabitMQProducer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        Task<bool> SendProductMessage<T>
        (
            T message,
            string exchangeName,
            string exchangeType = ExchangeType.Fanout,
            string routingKey = ""
        );
    }
}
