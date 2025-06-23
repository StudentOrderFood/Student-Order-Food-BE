using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class ShopRepository(AppDbContext context) : GenericRepository<Shop, Guid>(context), IShopRepository
    {
        public async Task<Shop?> GetShopByIdAsync(Guid shopId)
        {
            return await _context.Shops
                .Include(s => s.Owner)
                .FirstOrDefaultAsync(s => s.Id == shopId && !s.IsDeleted);
        }

        public async Task<IEnumerable<Shop>?> GetAllShopsAsync()
        {
            return await _context.Shops
                .Include(s => s.Owner)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Shop>?> GetShopsByStatusAsync(string status)
        {
            return await _context.Shops
                .Where(s => s.Status == status && !s.IsDeleted)
                .Include(s => s.Owner)
                .ToListAsync();
        }
    }
}
