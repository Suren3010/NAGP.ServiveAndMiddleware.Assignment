using NAGP.Orders.Service.Services.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Data.Common;
using System.Text;

namespace NAGP.Orders.Service.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RabitMQProducer : IRabitMQProducer
    {
        private readonly IConnection _rabbitMqConnection;

        /// <summary>
        /// Intialize instance for <see cref="RabitMQProducer"/>
        /// </summary>
        public RabitMQProducer
        (
            IConnection rabbitMqConnection
        ) => _rabbitMqConnection = rabbitMqConnection ?? throw new ArgumentNullException(nameof(rabbitMqConnection));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task<bool> SendProductMessage<T>
        (
            T message,
            string exchangeName,
            string exchangeType = ExchangeType.Fanout,
            string routingKey = ""
        )
        {
            return Task.Run(() =>
            {
                using (var channel = _rabbitMqConnection.CreateChannel())
                {
                    channel.ExchangeDeclare(exchangeName, exchangeType.ToString(), false, false, null);

                    var json = message as string;
                    ReadOnlyMemory<byte> body = Encoding.UTF8.GetBytes(json);

                    // Publish the message to the exchange
                    channel.BasicPublish(exchangeName, routingKey, body:body, mandatory: false);
                }
                return true;
            });

        }
    }
}
