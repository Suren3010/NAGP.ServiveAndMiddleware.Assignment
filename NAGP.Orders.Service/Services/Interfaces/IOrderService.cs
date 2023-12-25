namespace NAGP.Orders.Service.Services.Interfaces
{
    /// <summary>
    /// Contract to provide order related operations
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Create order based on provided details
        /// </summary>
        /// <param name="orderDetails">Order details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<OrderResponse> CreateOrder
        (
            OrderDetails orderDetails,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Update order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OrderResponse> UpdateOrder
        (
            int orderId,
            OrderDetails orderDetails,
            CancellationToken cancellationToken
        );
    }
}
