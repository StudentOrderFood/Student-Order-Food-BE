using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.MenuItem;
using OrderFood_BE.Application.Models.Response.MenuItem;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Application.UseCase.Interfaces.MenuItem;
using OrderFood_BE.Shared.Common;
using System.Net;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemUseCase _menuItemUseCase;
        private readonly ICloudinaryService _cloudinaryService;
        public MenuItemController(IMenuItemUseCase menuItemUseCase, ICloudinaryService cloudinaryService)
        {
            _menuItemUseCase = menuItemUseCase;
            _cloudinaryService = cloudinaryService;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetMenuItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateMenuItemAsync([FromForm] CreateMenuItemRequest request, IFormFile? image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image, "menu item");
                request.ImageUrl = imageUrl;
            }
            var rs = await _menuItemUseCase.CreateMenuItemAsync(request);
            return Ok(rs);

        }


        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetMenuItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateMenuItemAsync(Guid id, [FromForm] UpdateMenuItemRequest request, IFormFile? image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image, "menu item");
                request.ImageUrl = imageUrl;
            }
            var rs = await _menuItemUseCase.UpdateMenuItemAsync(id, request);
            return Ok(rs);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteMenuItemAsync(Guid id)
        {
            var rs = await _menuItemUseCase.DeleteMenuItemAsync(id);
            return Ok(rs);
        }

        [HttpGet("shop/{id}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetMenuItemResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMenuItemsByShopId(Guid id)
        {
            var rs = await _menuItemUseCase.GetMenuItemByShopIdAsync(id);
            return Ok(rs);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetMenuItemResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllMenuItems()
        {
            var rs = await _menuItemUseCase.GetAllMenuItemsAsync();
            return Ok(rs);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetMenuItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetMenuItemByIdAsync(Guid id)
        {
            var rs = await _menuItemUseCase.GetMenuItemByIdAsync(id);
            return Ok(rs);
        }
    }
}
