using Grpc.Core;
using NAGP.Product.Service.Services.Interfaces;

namespace NAGP.Product.Service.Controllers
{
    /// <summary>
    /// Controller to handle requests for product's order
    /// </summary>
    public class ProductController : Products.ProductsBase
    {
        private readonly IProductOrderService _productOrderService;

        /// <summary>
        /// Create instance for <see cref="ProductController"/>
        /// </summary>
        /// <param name="productOrderService"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductController
        (
            IProductOrderService productOrderService
        ) => _productOrderService = productOrderService ?? throw new ArgumentNullException(nameof(productOrderService));

        /// <summary>
        /// Serve requests to create orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProductOrderResponse> CreateOrder
        (
            ProductOrderDetails request,
            ServerCallContext context
        ) => _productOrderService.CreateOrder(request, CancellationToken.None);

        /// <summary>
        /// Serve requests to update orders
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ProductOrderResponse> UpdateOrder
        (
            ProductOrderDetails request,
            ServerCallContext context
        ) => _productOrderService.UpdateOrder(request.OrderId, request, CancellationToken.None);
    }
}
