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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
            IFormFile? image,
            List<IFormFile>? additionalImages)
        {
            //// Validate ảnh đại diện
            //if (image == null || image.Length == 0)
            //    return BadRequest(ApiResponse<GetShopResponse>.Fail("Image file is required."));

            try
            {
                // Lấy OwnerId từ JWT token
                var ownerIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (string.IsNullOrWhiteSpace(ownerIdClaim))
                    return Unauthorized(ApiResponse<GetShopResponse>.Fail("Unauthorized"));

                request.OwnerId = Guid.Parse(ownerIdClaim);

                if (image != null)
                {
                    // Upload ảnh đại diện
                    var imageUrl = await _cloudinaryService.UploadImageAsync(image, "shops");
                    request.ImageUrl = imageUrl;
                }

                // Tạo shop
                var result = await _shopUseCase.CreateShopAsync(request);

                // Upload ảnh phụ nếu có
                if (result.Success && additionalImages != null && additionalImages.Any())
                {
                    foreach (var img in additionalImages)
                    {
                        if (img.Length == 0) continue;

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
            catch (Exception ex)
            {
                // Có thể thêm logging ở đây nếu cần
                return StatusCode(500, ApiResponse<GetShopResponse>.Fail("Đã xảy ra lỗi khi tạo cửa hàng."));
            }
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
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<GetShopResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetShopByIdAsync(Guid shopId)
        {
            var shop = await _shopUseCase.GetShopByIdAsync(shopId);
            return Ok(shop);
        }

        [HttpGet("shop-owner")]
        [Authorize(Roles = "ShopOwner")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetShopResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetShopsByOwnerAsync([FromQuery] PagingRequest request)
        {
            var ownerIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrWhiteSpace(ownerIdClaim))
                return Unauthorized(ApiResponse<PagingResponse<GetShopResponse>>.Fail("Unauthorized"));

            var ownerId = Guid.Parse(ownerIdClaim);

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
            try
            {
                // Lấy OwnerId từ JWT
                var ownerIdClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
                if (string.IsNullOrWhiteSpace(ownerIdClaim))
                    return Unauthorized(ApiResponse<GetShopResponse>.Fail("Unauthorized"));

                var ownerId = Guid.Parse(ownerIdClaim);

                // Cập nhật ảnh đại diện nếu có
                if (image != null && image.Length > 0)
                {
                    var imageUrl = await _cloudinaryService.UploadImageAsync(image, "shops");
                    request.ImageUrl = imageUrl;
                }

                // Gán ownerId vào request để kiểm tra trong UseCase (nếu cần)
                request.OwnerId = ownerId;

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
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                Console.WriteLine("ERROR in CreateShopAsync: " + ex.ToString());
                return StatusCode(500, ApiResponse<GetShopResponse>.Fail("Đã xảy ra lỗi khi cập nhật cửa hàng."));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "ShopOwner")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteShopAsync(Guid shopId)
        {
            var result = await _shopUseCase.DeleteShopAsync(shopId);
            return Ok(result);
        }

        [HttpGet("detail/{shopId:guid}")]
        public async Task<IActionResult> GetShopDetail(Guid shopId)
        {
            var result = await _shopUseCase.GetShopIncludeItemsAndCategoryByIdAsync(shopId);
            return Ok(result);
        }

        [HttpGet("popular")]
        public async Task<IActionResult> GetPopularShop(string currentTime)
        {
            var result = await _shopUseCase.GetPopularShopAsync(currentTime);
            return Ok(result);
        }


    }
}
