using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.Models.Response.Shop;
using OrderFood_BE.Application.Services;
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
        private readonly ICloudinaryService _cloudinaryService;

        public ShopsController(IShopUseCase shopUseCase, ICloudinaryService cloudinaryService)
        {
            _shopUseCase = shopUseCase;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        [Authorize(Roles = "ShopOwner")]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateShopAsync(
            [FromForm] CreateShopRequest request,
            IFormFile image,
            List<IFormFile>? additionalImages)
        {
            if (image == null || image.Length == 0)
                return BadRequest(ApiResponse<GetShopResponse>.Fail("Image file is required."));

            // Upload ảnh bìa
            var imageUrl = await _cloudinaryService.UploadImageAsync(image, "shops");
            request.ImageUrl = imageUrl;

            // Tạo shop
            var result = await _shopUseCase.CreateShopAsync(request);

            // Nếu thành công thì upload ảnh phụ
            if (result.Success && additionalImages != null && additionalImages.Any())
            {
                foreach (var img in additionalImages)
                {
                    var subImageUrl = await _cloudinaryService.UploadImageAsync(img, "shops/gallery");

                    await _shopUseCase.AddShopImageAsync(new UpdateShopImageRequest
                    {
                        ShopId = result.Data.Id,
                        ImageUrl = subImageUrl
                    });
                }
            }

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

        [HttpGet("{shopId}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetShopByIdAsync(Guid shopId)
        {
            var shop = await _shopUseCase.GetShopByIdAsync(shopId);
            return Ok(shop);
        }

        [HttpGet("shop-owner/{ownerId}")]
        [Authorize(Roles = "ShopOwner")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetShopsByOwnerAsync(Guid ownerId, [FromQuery] PagingRequest request)
        {
            var shops = await _shopUseCase.GetShopsByOwnerIdAsync(ownerId, request);
            return Ok(shops);
        }

        [HttpPut]
        [Authorize(Roles = "ShopOwner")]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateShopAsync(
            [FromForm] UpdateShopRequest request,
            IFormFile? image,
            List<IFormFile>? additionalImages)
        {
            // Cập nhật ảnh bìa nếu có
            if (image != null && image.Length > 0)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image, "shops");
                request.ImageUrl = imageUrl;
            }

            // Cập nhật thông tin shop
            var result = await _shopUseCase.UpdateShopAsync(request);

            // Nếu thành công thì xử lý ảnh phụ
            if (result.Success && additionalImages != null && additionalImages.Any())
            {
                foreach (var img in additionalImages)
                {
                    if (img.Length == 0) continue;

                    var subImageUrl = await _cloudinaryService.UploadImageAsync(img, "shops/gallery");
                    await _shopUseCase.AddShopImageAsync(new UpdateShopImageRequest
                    {
                        ShopId = request.ShopId,
                        ImageUrl = subImageUrl
                    });
                }
            }

            return Ok(result);
        }


        [HttpDelete]
        [Authorize(Roles = "ShopOwner")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteShopAsync(Guid shopId)
        {
            var result = await _shopUseCase.DeleteShopAsync(shopId);
            return Ok(result);
        }

    }
}
