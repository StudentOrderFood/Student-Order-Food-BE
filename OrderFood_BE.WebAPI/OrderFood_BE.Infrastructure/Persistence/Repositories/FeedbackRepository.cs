using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class FeedbackRepository(AppDbContext context) : GenericRepository<Feedback, Guid>(context), IFeedbackRepository
    {
        public async Task<IEnumerable<Feedback>> GetFeedbacksByShopIdAsync(Guid shopId)
        {
            return await _context.Feedbacks
                .Include(f => f.Customer)
                .Include(f => f.Order)
                .Where(f => f.Order.ShopId == shopId && !f.IsDeleted && f.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByCustomerIdAsync(Guid customerId)
        {
            return await _context.Feedbacks
                .Where(f => f.CustomerId == customerId && !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByOrderIdAsync(Guid orderId)
        {
            return await _context.Feedbacks
                .Where(f => f.OrderId == orderId && !f.IsDeleted)
                .FirstOrDefaultAsync();
        }
    }
}
