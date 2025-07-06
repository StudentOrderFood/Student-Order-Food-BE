using OrderFood_BE.Application.Models.Requests.MenuItem;
using OrderFood_BE.Application.Models.Response.MenuItem;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.MenuItem;
using OrderFood_BE.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.UseCase.Implementations.MenuItem
{
    public class MenuItemUseCase : IMenuItemUseCase
    {
        private readonly IMenuItemRepository _menuItemRepository;
        public MenuItemUseCase(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }
        public async Task<ApiResponse<GetMenuItemResponse>> CreateMenuItemAsync(CreateMenuItemRequest request)
        {
            if (request == null)
                return ApiResponse<GetMenuItemResponse>.Fail("Bad request!");
            var newMenuItem = new Domain.Entities.MenuItem
            {
                Name = request.Name,
                CategoryId = request.CategoryId,
                ImageUrl = request.ImageUrl,
                Description = request.Description,
                IsAvailable = true,
                Price = request.Price,
                ShopId = request.ShopId,
                CreatedAt = DateTime.Now,

            };
            await _menuItemRepository.AddAsync(newMenuItem);
            await _menuItemRepository.SaveChangesAsync();
            var response = new GetMenuItemResponse
            {
                Name = newMenuItem.Name,
                CategoryId = newMenuItem.CategoryId,
                ShopId = newMenuItem.ShopId,
                Price = newMenuItem.Price,
                Description = newMenuItem.Description,
                IsAvailable = newMenuItem.IsAvailable,
                ImageUrl = newMenuItem.ImageUrl,


            };
            return ApiResponse<GetMenuItemResponse>.Ok(response, "Create menu item successfully");


        }


        public async Task<ApiResponse<string>> DeleteMenuItemAsync(Guid id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
                return ApiResponse<string>.Fail("Not found this menu item");
            await _menuItemRepository.SoftDeleteAsync(menuItem);
            await _menuItemRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("", "Delete menu item successfully");
        }

        public async Task<ApiResponse<IEnumerable<GetMenuItemResponse>>> GetAllMenuItemsAsync()
        {
            var result = await _menuItemRepository.GetAllMenuItemsAsync();
            var responseList = result.Select(m => new GetMenuItemResponse
            {
                Id = m.Id,
                Name = m.Name,
                CategoryId = m.CategoryId,
                ShopId = m.ShopId,
                Price = m.Price,
                Description = m.Description,
                IsAvailable = m.IsAvailable,
                ImageUrl = m.ImageUrl,
            });

            return ApiResponse<IEnumerable<GetMenuItemResponse>>.Ok(responseList, "List of all menu item of shop");
        }

        public async Task<ApiResponse<GetMenuItemResponse>> GetMenuItemByIdAsync(Guid id)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem is null)
                return ApiResponse<GetMenuItemResponse>.Fail("This menu item is not exist");
            var response = new GetMenuItemResponse
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                CategoryId = menuItem.CategoryId,
                ShopId = menuItem.ShopId,
                Price = menuItem.Price,
                Description = menuItem.Description,
                IsAvailable = menuItem.IsAvailable,
                ImageUrl = menuItem.ImageUrl,
            };

            return ApiResponse<GetMenuItemResponse>.Ok(response, "Found menu item");
        }

        public async Task<ApiResponse<IEnumerable<GetMenuItemResponse>>> GetMenuItemByShopIdAsync(Guid shopId)
        {
            var result = await _menuItemRepository.GetMenuItemByShopIdAsync(shopId);
            var responseList = result.Select(m => new GetMenuItemResponse
            {
                Id = m.Id,
                Name = m.Name,
                CategoryId = m.CategoryId,
                ShopId = m.ShopId,
                Price = m.Price,
                Description = m.Description,
                IsAvailable = m.IsAvailable,
                ImageUrl = m.ImageUrl,
            });


            return ApiResponse<IEnumerable<GetMenuItemResponse>>.Ok(responseList, "List of menu item of shop");
        }

        public async Task<ApiResponse<GetMenuItemResponse>> UpdateMenuItemAsync(Guid id, UpdateMenuItemRequest request)
        {
            var menuItem = await _menuItemRepository.GetByIdAsync(id);
            if (menuItem == null)
                return ApiResponse<GetMenuItemResponse>.Fail("This menu item is not exist");
            menuItem.Name = request.Name;
            menuItem.CategoryId = request.CategoryId;
            menuItem.ShopId = request.ShopId;
            menuItem.Price = request.Price;
            menuItem.Description = request.Description;
            menuItem.IsAvailable = request.IsAvailable;
            menuItem.ImageUrl = request.ImageUrl;
            menuItem.UpdatedAt = DateTime.Now;
            await _menuItemRepository.UpdateAsync(menuItem);
            await _menuItemRepository.SaveChangesAsync();
            var response = new GetMenuItemResponse
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                CategoryId = menuItem.CategoryId,
                ShopId = menuItem.ShopId,
                Price = menuItem.Price,
                Description = menuItem.Description,
                IsAvailable = menuItem.IsAvailable,
                ImageUrl = menuItem.ImageUrl,


            };
            return ApiResponse<GetMenuItemResponse>.Ok(response, "Update menu item successfully");

        }
    }
}
