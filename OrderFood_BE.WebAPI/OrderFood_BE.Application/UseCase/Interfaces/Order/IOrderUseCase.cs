using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.Models.Response.Order;
using OrderFood_BE.Application.Models.Response.Payment;
using OrderFood_BE.Domain.Entities;
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
        Task<ApiResponse<string>> CreateOrderFromFirebaseAsync(string firebaseId, OrderRequestFireBase request);
        Task<int> UpdateOrderStatusFromFirebaseAsync(string firebaseId, string newStatus);
        Task<int> HandleSuccessfulPaymentAsync(BankingOrderRequest model);
    }
}
