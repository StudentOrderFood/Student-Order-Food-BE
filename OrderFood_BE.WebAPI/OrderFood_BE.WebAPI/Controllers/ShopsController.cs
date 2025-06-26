using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.Models.Response.Shop;
using OrderFood_BE.Application.UseCase.Interfaces.Shop;
using OrderFood_BE.Shared.Common;
using System.Net;

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
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateShopAsync([FromBody] CreateShopRequest request)
        {
            var result = await _shopUseCase.CreateShopAsync(request);
            return Ok(result);
        }

        [HttpPost("approve-reject")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ApproveOrRejectShopAsync([FromBody] ApproveShopRequest request)
        {
            var result = await _shopUseCase.ApproveOrRejectShopAsync(request);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetShopsByStatusAsync(string status, [FromQuery] PagingRequest request)
        {
            var shops = await _shopUseCase.GetShopsByStatusAsync(status, request);
            return Ok(shops);
        }
    }
}
