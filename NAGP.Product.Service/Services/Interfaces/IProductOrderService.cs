namespace NAGP.Product.Service.Services.Interfaces
{
    /// <summary>
    /// Contract to product order operations
    /// </summary>
    public interface IProductOrderService
    {
        /// <summary>
        /// Create order based on provided details
        /// </summary>
        /// <param name="orderDetails">Order details</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        Task<ProductOrderResponse> CreateOrder
        (
            ProductOrderDetails orderDetails,
            CancellationToken cancellationToken
        );

        /// <summary>
        /// Update order 
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<ProductOrderResponse> UpdateOrder
        (
            int orderId,
            ProductOrderDetails orderDetails,
            CancellationToken cancellationToken
        );
    }
}
