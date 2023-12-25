using NAGP.Orders.Service.Services.Interfaces;
using RabbitMQ.Client;

namespace NAGP.Orders.Service.Services
{
    /// <summary>
    /// Class to provide order related operations 
    /// </summary>
    public class OrderService : IOrderService
    {
        private const string ORDER_EXCHANGE_FANOUT = "order-exchange-fanout";
        private const string ORDER_EXCHANGE_TOPIC = "order-exchange-topic";
        private readonly ILogger<OrderService> _logger;
        private readonly IRabitMQProducer _rabbitMQProducer;

        /// <summary>
        /// Create instance for <see cref="OrderService"/>
        /// </summary>
        public OrderService
        (
            IRabitMQProducer rabbitMQProducer,
            ILogger<OrderService> logger
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rabbitMQProducer = rabbitMQProducer ?? throw new ArgumentNullException(nameof(rabbitMQProducer));
        }

        /// <summary>
        /// Create order based on provided details
        /// </summary>
        /// <param name="orderDetails">Order details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<OrderResponse> CreateOrder
        (
            OrderDetails orderDetails,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            var notificationMessage = $"Order created for product --> product id : {orderDetails.Product.Id}, name: {orderDetails.Product.Name}, quantity: {orderDetails.Quantity}";
            _logger.LogInformation(notificationMessage);
            //**** Send notifications to rabbitMq for order creation
            _ = await _rabbitMQProducer.SendProductMessage<string>(notificationMessage, ORDER_EXCHANGE_FANOUT,routingKey: "Order.Create");
            return new OrderResponse
            {
                IsSuccess = true,
                Message = notificationMessage
            };
        }

        /// <summary>
        /// Update order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<OrderResponse> UpdateOrder
        (
            int orderId,
            OrderDetails orderDetails,
            CancellationToken cancellationToken
        )
        {
            var notificationMessage = $"Order update for order id: {orderId} and product details are --> product id : {orderDetails.Product.Id}, name: {orderDetails.Product.Name}, quantity: {orderDetails.Quantity}";
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation(notificationMessage);
            //**** Send notifications to rabbitMq for order updates
            _ = await _rabbitMQProducer.SendProductMessage<string>(notificationMessage, ORDER_EXCHANGE_TOPIC, ExchangeType.Topic, "Order.Update");
            return new OrderResponse
            {
                IsSuccess = true,
                Message = notificationMessage
            };
        }
    }
}
