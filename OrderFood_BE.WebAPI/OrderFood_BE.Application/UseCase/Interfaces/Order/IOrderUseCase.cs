using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.Models.Response.Order;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Interfaces.Order
{
    public interface IOrderUseCase
    {
        Task<ApiResponse<GetOrderResponse>> CreateOrderAsync(CreateOrderRequest request);
        Task<ApiResponse<GetOrderResponse>> GetOrderByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<GetOrderResponse>>> GetOrdersByCustomerIdAsync(Guid customerId);
        Task<ApiResponse<IEnumerable<GetOrderResponse>>> GetOrdersByShopIdAsync(Guid shopId);
        Task<ApiResponse<string>> CancelOrderAsync(Guid id);
    }
}
