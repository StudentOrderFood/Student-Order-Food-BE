using OrderFood_BE.Application.Models.Requests.Category;
using OrderFood_BE.Application.Models.Response.Category;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Category;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Implementations.Category
{
    public class CategoryUseCase : ICategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ApiResponse<GetCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var newCategory = new Domain.Entities.Category
            {
                Name = request.Name,
                Description = request.Description,
            };

            await _categoryRepository.AddAsync(newCategory);
            await _categoryRepository.SaveChangesAsync();

            return ApiResponse<GetCategoryResponse>.Ok(new GetCategoryResponse
            {
                Id = newCategory.Id,
                Name = newCategory.Name,
                Description = newCategory.Description
            }, "Category created successfully");
        }

        public async Task<ApiResponse<GetCategoryResponse>> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return ApiResponse<GetCategoryResponse>.Fail("Category not found");

            category.Name = request.Name;
            category.Description = request.Description;

            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return ApiResponse<GetCategoryResponse>.Ok(new GetCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            }, "Category updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return ApiResponse<string>.Fail("Category not found");

            await _categoryRepository.SoftDeleteAsync(category);
            await _categoryRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("", "Category deleted successfully");
        }

        public async Task<ApiResponse<GetCategoryResponse>> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return ApiResponse<GetCategoryResponse>.Fail("Category not found");

            return ApiResponse<GetCategoryResponse>.Ok(new GetCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            }, "Category found");
        }

        public async Task<ApiResponse<IEnumerable<GetCategoryResponse>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();

            var result = categories.Select(c => new GetCategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });

            return ApiResponse<IEnumerable<GetCategoryResponse>>.Ok(result, "List of categories");
        }
    }
}
