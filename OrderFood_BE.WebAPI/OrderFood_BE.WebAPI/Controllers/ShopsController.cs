using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.UseCase.Interfaces.Shop;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ShopsController : ControllerBase
    {
        private readonly IShopUseCase _shopUseCase;
        public ShopsController(IShopUseCase shopUseCase)
        {
            _shopUseCase = shopUseCase;
        }
        [HttpPost]
        [Authorize(Roles = "ShopOwner")]
        public async Task<IActionResult> CreateShopAsync([FromBody] CreateShopRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            var result = await _shopUseCase.CreateShopAsync(request);
            if (result == "User is not authorized to create a shop.")
            {
                return Forbid(result);
            }
            return Ok(result);
        }

        [HttpPost("approve-reject")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveOrRejectShopAsync([FromBody] ApproveShopRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }
            var result = await _shopUseCase.ApproveOrRejectShopAsync(request);
            if (result == "Shop not found." || result == "This shop has already been processed.")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetShopsByStatusAsync(string status, [FromQuery] PagingRequest request)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Status cannot be null or empty.");
            }
            var shops = await _shopUseCase.GetShopsByStatusAsync(status, request);
            if (shops == null || !shops.Items.Any())
            {
                return NotFound("No shops found with the specified status.");
            }
            return Ok(shops);
        }
    }
}
