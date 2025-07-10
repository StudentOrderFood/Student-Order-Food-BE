using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class OrderRepository(AppDbContext context) : GenericRepository<Order, Guid>(context), IOrderRepository
    {
        public async Task<IEnumerable<Order>> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId && !o.IsDeleted)
                .Include(o => o.OrderItems)
                .Include(o => o.Shop)
                .Include(o => o.Customer)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersByShopIdAsync(Guid shopId)
        {
            return await _context.Orders
                .Where(o => o.ShopId == shopId && !o.IsDeleted)
                .Include(o => o.OrderItems)
                .Include(o => o.Customer)
                .Include(o => o.Shop)
                .ToListAsync();
        }

        public async Task<Order> GetOrderWithItemsAsync(Guid orderId)
        {
            return await _context.Orders
                .Where(o => o.Id == orderId && !o.IsDeleted)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync();
        }
    }
}
