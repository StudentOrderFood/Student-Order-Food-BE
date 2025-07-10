using OrderFood_BE.Application.Models.Requests.Feedback;
using OrderFood_BE.Application.Models.Response.Feedback;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Interfaces.Feedback
{
    public interface IFeedbackUseCase
    {
        Task<ApiResponse<GetFeedbackResponse>> CreateFeedbackAsync(CreateFeedbackRequest request);
        Task<ApiResponse<IEnumerable<GetFeedbackResponse>>> GetFeedbacksByShopIdAsync(Guid shopId);
        Task<ApiResponse<IEnumerable<GetFeedbackResponse>>> GetFeedbacksByCustomerIdAsync(Guid customerId);
        Task<ApiResponse<GetFeedbackResponse>> GetFeedbackByOrderIdAsync(Guid orderId);
    }
}
