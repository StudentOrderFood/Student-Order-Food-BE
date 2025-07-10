using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category, Guid>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetByIdAsync(Guid id);
        Task<Category> GetByNameAsync(string name);
    }
}
