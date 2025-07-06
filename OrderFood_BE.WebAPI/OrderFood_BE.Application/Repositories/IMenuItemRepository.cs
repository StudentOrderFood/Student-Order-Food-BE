using OrderFood_BE.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Repositories
{
    public interface IMenuItemRepository : IGenericRepository<MenuItem, Guid>
    {
        Task<IEnumerable<MenuItem>?> GetMenuItemByShopIdAsync(Guid id);
        Task<IEnumerable<MenuItem>?> GetAllMenuItemsAsync();

    }
}
