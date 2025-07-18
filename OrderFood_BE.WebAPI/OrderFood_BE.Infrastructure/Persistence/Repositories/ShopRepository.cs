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
                .Include(s => s.ShopImages)
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

        public async Task AddShopImageAsync(ShopImage shopImage)
        {
            await _context.ShopImages.AddAsync(shopImage);
        }

        public Task<Shop?> GetShopDetailByIdAsync(Guid shopId, bool includeMenuItems, bool includeCategoryItems)
        {
            var query = _context.Shops.AsQueryable();
            if (includeCategoryItems)
            {
                query = query
                    .Include(s => s.MenuItems
                        .Where(mi => !mi.IsDeleted && mi.IsAvailable) // Filter MenuItems
                        ) 
                    .ThenInclude(mi => mi.Category);
                return query
                    .Include(s => s.ShopImages)
                    .FirstOrDefaultAsync(s => s.Id == shopId && !s.IsDeleted);
            }

            if (includeMenuItems)
            {
                query = query.Include(s => s.MenuItems
                    .Where(mi => !mi.IsDeleted && mi.IsAvailable)); // Filter MenuItems
            }
            return query
                .Include(s => s.ShopImages)
                .FirstOrDefaultAsync(s => s.Id == shopId && !s.IsDeleted);
        }

        public async Task<IEnumerable<Shop>> GetPopularShopsByTimeAndMealAsync(TimeSpan currentTime, List<string> mealTypes)
        {
            return await _context.Shops
                .Include(s => s.MenuItems)
                    .ThenInclude(mi => mi.Category)
                .Where(s =>
                    s.OpenHours <= currentTime &&
                    s.EndHours >= currentTime &&
                    s.MenuItems.Any(mi =>
                        mi.IsAvailable &&
                        mealTypes.Contains(mi.Category.Name)
                    )
                )
                .OrderByDescending(s => s.Rating)
                .ToListAsync();
        }
    }
}
