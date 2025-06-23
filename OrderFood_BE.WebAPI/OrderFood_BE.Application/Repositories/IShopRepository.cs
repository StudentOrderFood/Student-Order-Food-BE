using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop, Guid>
    {
        Task<Shop?> GetShopByIdAsync(Guid shopId);
        Task<IEnumerable<Shop>?> GetAllShopsAsync();
        Task<IEnumerable<Shop>?> GetShopsByStatusAsync(string status);
    }
}
