using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class MenuItemRepository(AppDbContext context) : GenericRepository<MenuItem, Guid>(context), IMenuItemRepository
    {

        public async Task<IEnumerable<MenuItem>?> GetAllMenuItemsAsync()
        {
            return await _context.MenuItems
                .Include(c => c.Category)
                .ToListAsync();
        }


        public async Task<IEnumerable<MenuItem>?> GetMenuItemByShopIdAsync(Guid shopId)
        {
            return await _context.MenuItems
                .Include(c => c.Category)
                .Where(m => m.ShopId == shopId && !m.IsDeleted)
                .ToListAsync();
        }
    }
}
