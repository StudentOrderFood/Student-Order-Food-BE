using OrderFood_BE.Application.Models.Requests.Feedback;
using OrderFood_BE.Application.Models.Response.Feedback;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Feedback;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Implementations.Feedback
{
    public class FeedbackUseCase : IFeedbackUseCase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackUseCase(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        public async Task<ApiResponse<GetFeedbackResponse>> CreateFeedbackAsync(CreateFeedbackRequest request)
        {
            var exists = await _feedbackRepository.GetFeedbackByOrderIdAsync(request.OrderId);
            if (exists != null)
                return ApiResponse<GetFeedbackResponse>.Fail("Feedback already exists for this order.");

            var feedback = new Domain.Entities.Feedback
            {
                OrderId = request.OrderId,
                CustomerId = request.CustomerId,
                Rating = request.Rating,
                Content = request.Content,
                ImageUrl = request.ImageUrl ?? "",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _feedbackRepository.AddAsync(feedback);
            await _feedbackRepository.SaveChangesAsync();

            var response = new GetFeedbackResponse
            {
                Id = feedback.Id,
                OrderId = feedback.OrderId,
                CustomerId = feedback.CustomerId,
                Rating = feedback.Rating,
                Content = feedback.Content,
                ImageUrl = feedback.ImageUrl,
                IsActive = feedback.IsActive,
                CreatedAt = feedback.CreatedAt
            };

            return ApiResponse<GetFeedbackResponse>.Ok(response, "Feedback submitted");
        }

        public async Task<ApiResponse<IEnumerable<GetFeedbackResponse>>> GetFeedbacksByShopIdAsync(Guid shopId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByShopIdAsync(shopId);
            var result = feedbacks.Select(f => new GetFeedbackResponse
            {
                Id = f.Id,
                OrderId = f.OrderId,
                CustomerId = f.CustomerId,
                Rating = f.Rating,
                Content = f.Content,
                ImageUrl = f.ImageUrl,
                IsActive = f.IsActive,
                CreatedAt = f.CreatedAt
            });

            return ApiResponse<IEnumerable<GetFeedbackResponse>>.Ok(result, "List of feedbacks for shop");
        }

        public async Task<ApiResponse<IEnumerable<GetFeedbackResponse>>> GetFeedbacksByCustomerIdAsync(Guid customerId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByCustomerIdAsync(customerId);
            var result = feedbacks.Select(f => new GetFeedbackResponse
            {
                Id = f.Id,
                OrderId = f.OrderId,
                CustomerId = f.CustomerId,
                Rating = f.Rating,
                Content = f.Content,
                ImageUrl = f.ImageUrl,
                IsActive = f.IsActive,
                CreatedAt = f.CreatedAt
            });

            return ApiResponse<IEnumerable<GetFeedbackResponse>>.Ok(result, "Customer feedbacks retrieved");
        }

        public async Task<ApiResponse<GetFeedbackResponse>> GetFeedbackByOrderIdAsync(Guid orderId)
        {
            var f = await _feedbackRepository.GetFeedbackByOrderIdAsync(orderId);
            if (f == null)
                return ApiResponse<GetFeedbackResponse>.Fail("No feedback found for this order");

            var response = new GetFeedbackResponse
            {
                Id = f.Id,
                OrderId = f.OrderId,
                CustomerId = f.CustomerId,
                Rating = f.Rating,
                Content = f.Content,
                ImageUrl = f.ImageUrl,
                IsActive = f.IsActive,
                CreatedAt = f.CreatedAt
            };

            return ApiResponse<GetFeedbackResponse>.Ok(response, "Get feedback by order success");
        }
    }
}
