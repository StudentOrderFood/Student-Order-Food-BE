using OrderFood_BE.Application.Models.Requests.MenuItem;
using OrderFood_BE.Application.Models.Response.MenuItem;
using OrderFood_BE.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.UseCase.Interfaces.MenuItem
{
    public interface IMenuItemUseCase
    {
        Task<ApiResponse<GetMenuItemResponse>> CreateMenuItemAsync(CreateMenuItemRequest request);
        Task<ApiResponse<IEnumerable<GetMenuItemResponse>>> GetMenuItemByShopIdAsync(Guid shopId);
        Task<ApiResponse<IEnumerable<GetMenuItemResponse>>> GetAllMenuItemsAsync();
        Task<ApiResponse<GetMenuItemResponse>> GetMenuItemByIdAsync(Guid id);
        Task<ApiResponse<GetMenuItemResponse>> UpdateMenuItemAsync(Guid id, UpdateMenuItemRequest request);
        Task<ApiResponse<string>> DeleteMenuItemAsync(Guid id);
    }
}
