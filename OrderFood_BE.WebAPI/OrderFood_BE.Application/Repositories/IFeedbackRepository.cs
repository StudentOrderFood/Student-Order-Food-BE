using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface IFeedbackRepository : IGenericRepository<Feedback, Guid>
    {
        Task<IEnumerable<Feedback>> GetFeedbacksByShopIdAsync(Guid shopId);
        Task<IEnumerable<Feedback>> GetFeedbacksByCustomerIdAsync(Guid customerId);
        Task<Feedback> GetFeedbackByOrderIdAsync(Guid orderId);
    }
}
