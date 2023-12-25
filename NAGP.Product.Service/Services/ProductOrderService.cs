using Grpc.Net.ClientFactory;
using NAGP.Product.Service.Services.Interfaces;

namespace NAGP.Product.Service.Services
{
    /// <summary>
    /// Service for product order related operations
    /// </summary>
    public class ProductOrderService : IProductOrderService
    {
        private readonly ILogger<ProductOrderService> _logger;
        private readonly Orders.OrdersClient _grpcOrderClient;

        /// <summary>
        /// Create instance for <see cref="ProductOrderService"/>
        /// </summary>
        /// <param name="grpcClientFactory"></param>
        /// <param name="logger"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductOrderService
        (
            GrpcClientFactory grpcClientFactory,
            ILogger<ProductOrderService> logger
        )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _grpcOrderClient = grpcClientFactory.CreateClient<Orders.OrdersClient>("OrderClient") 
                ?? throw new ArgumentNullException(nameof(grpcClientFactory));
        }

        /// <summary>
        /// Create order based on provided details
        /// </summary>
        /// <param name="orderDetails">Order details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        public async Task<ProductOrderResponse> CreateOrder
        (
            ProductOrderDetails orderDetails,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation($"Order created for product --> product id : {orderDetails.Product.Id}, quantity: {orderDetails.Quantity}");
            //*** Call order service to create order
            var clientOrderDetails = new OrderDetails
            {
                Product = new ProductDetails
                {
                    Id = orderDetails.Product.Id,
                    Name = orderDetails.Product.Name,
                    Description = orderDetails.Product.Description,
                    Color = orderDetails.Product.Color
                },
                Quantity = orderDetails.Quantity
            };
            var response = await _grpcOrderClient.CreateOrderAsync(clientOrderDetails);
            return new ProductOrderResponse
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message
            }; 
        }

        /// <summary>
        /// Update order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ProductOrderResponse> UpdateOrder
        (
            int orderId,
            ProductOrderDetails orderDetails,
            CancellationToken cancellationToken
        )
        {
            cancellationToken.ThrowIfCancellationRequested();
            _logger.LogInformation($"Order update for order id: {orderId} and product details are --> product id : {orderDetails.Product.Id}, quantity: {orderDetails.Quantity}");
            //***** Call order service to update order details
            var clientOrderDetails = new OrderDetails
            {
                Product = new ProductDetails
                {
                    Id = orderDetails.Product.Id,
                    Name = orderDetails.Product.Name,
                    Description = orderDetails.Product.Description,
                    Color = orderDetails.Product.Color
                },
                Quantity = orderDetails.Quantity,
                Id = orderDetails.OrderId
            };
            var response = await _grpcOrderClient.UpdateOrderAsync(clientOrderDetails);
            return new ProductOrderResponse
            {
                IsSuccess = response.IsSuccess,
                Message = response.Message
            };
        }

    }
}
