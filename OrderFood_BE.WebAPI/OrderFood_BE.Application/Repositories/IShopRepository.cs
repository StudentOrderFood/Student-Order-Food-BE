using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface IShopRepository : IGenericRepository<Shop, Guid>
    {
        Task<Shop?> GetShopByIdAsync(Guid shopId);
        Task<IEnumerable<Shop>?> GetAllShopsAsync();
        Task<IEnumerable<Shop>?> GetShopsByStatusAsync(string status);
        Task AddShopImageAsync(ShopImage shopImage);
        /// <summary>
        /// Lấy chi tiết cửa hàng theo ID, bao gồm các item trong menu và các loại items.
        /// Nếu includeCategoryItems là true, sẽ bao gồm includeMenuItems = true.
        /// Còn nếu includeMenuItems = true thì includeCategoryItems có thể true hoặc false
        /// </summary>
        /// <param name="shopId"></param>
        /// <param name="includeMenuItems"></param>
        /// <param name="includeCategoryItems"></param>
        /// <returns></returns>
        Task<Shop?> GetShopDetailByIdAsync(Guid shopId, bool includeMenuItems, bool includeCategoryItems);
        Task<IEnumerable<Shop>> GetPopularShopsByTimeAndMealAsync(TimeSpan currentTime, List<string> mealType);
    }
}
