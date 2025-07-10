using OrderFood_BE.Application.Models.Requests.Category;
using OrderFood_BE.Application.Models.Response.Category;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Interfaces.Category
{
    public interface ICategoryUseCase
    {
        Task<ApiResponse<GetCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request);
        Task<ApiResponse<GetCategoryResponse>> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
        Task<ApiResponse<string>> DeleteCategoryAsync(Guid id);
        Task<ApiResponse<GetCategoryResponse>> GetCategoryByIdAsync(Guid id);
        Task<ApiResponse<IEnumerable<GetCategoryResponse>>> GetAllCategoriesAsync();
    }
}
