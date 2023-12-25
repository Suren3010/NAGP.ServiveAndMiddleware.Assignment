using Grpc.Core;
using NAGP.Orders.Service.Services.Interfaces;

namespace NAGP.Orders.Service.Controllers
{
    /// <summary>
    /// Controller to handle requests for orders 
    /// </summary>
    public class OrderController : Orders.OrdersBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Create instance for <see cref="OrderController"/>
        /// </summary>
        public OrderController
        (
            IOrderService orderService
        ) => _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));

        /// <summary>
        /// Serve requests to create orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<OrderResponse> CreateOrder
        (
            OrderDetails request,
            ServerCallContext context
        ) => _orderService.CreateOrder(request, CancellationToken.None);

        /// <summary>
        /// Serve requests to update orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<OrderResponse> UpdateOrder
        (
            OrderDetails request,
            ServerCallContext context
        ) => _orderService.UpdateOrder(request.Id, request, CancellationToken.None);
    }
}
