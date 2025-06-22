using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class ShopRepository(AppDbContext context) : GenericRepository<Shop, Guid>(context), IShopRepository
    {
    }
}
