using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.Models.Response.Shop;
using OrderFood_BE.Application.Models.Response.User;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Shop;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Shared.Common;
using OrderFood_BE.Shared.Enums;
using System.Net;

namespace OrderFood_BE.Application.UseCase.Implementations.Shop
{
    public class ShopUseCase : IShopUseCase
    {
        private readonly IShopRepository _shopRepository;
        private readonly IUserRepository _userRepository;
        public ShopUseCase(IShopRepository shopRepository, IUserRepository userRepository)
        {
            _shopRepository = shopRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<GetShopResponse>> CreateShopAsync(CreateShopRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.OwnerId);

            if (user == null || user.Role.Name != RoleEnum.ShopOwner.ToString())
            {
                return ApiResponse<GetShopResponse>.Fail("User is not authorized to create a shop.");
            }

            var shop = new Domain.Entities.Shop
            {
                Name = request.Name,
                ImageUrl = request.ImageUrl,
                Address = request.Address,
                Status = ShopEnumStatus.Pending.ToString(), // Pending approval
                OpenHours = request.OpenHours,
                EndHours = request.EndHours,
                Rating = 0, // Default rating
                OwnerId = request.OwnerId
            };

            await _shopRepository.AddAsync(shop);
            await _shopRepository.SaveChangesAsync();

            var shopResponse = new GetShopResponse
            {
                Id = shop.Id,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Address = shop.Address,
                Status = shop.Status,
                OpenHours = shop.OpenHours,
                EndHours = shop.EndHours,
                Rating = shop.Rating,
                OwnerId = shop.OwnerId,
                Owner = new GetUserResponse
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    Phone = user.Phone,
                    Address = user.Address,
                    Avatar = user.Avatar,
                    Email = user.Email,
                    Dob = user.Dob,
                    RoleId = user.RoleId,
                    RoleName = user.Role?.Name ?? string.Empty
                }
            };

            return ApiResponse<GetShopResponse>.Ok(shopResponse, "The request has been submitted and is awaiting approval.");
        }

        public async Task<ApiResponse<string>> ApproveOrRejectShopAsync(ApproveShopRequest request)
        {
            var shop = await _shopRepository.GetShopByIdAsync(request.ShopId);

            if (shop == null)
                return ApiResponse<string>.Fail("Không tìm thấy cửa hàng.");

            if (shop.Status != ShopEnumStatus.Pending.ToString())
                return ApiResponse<string>.Fail("This shop has already been processed.");

            shop.Status = request.IsApproved
                ? ShopEnumStatus.Approved.ToString()
                : ShopEnumStatus.Rejected.ToString();

            await _shopRepository.UpdateAsync(shop);
            await _shopRepository.SaveChangesAsync();

            var message = request.IsApproved
                ? "Shop approved successfully."
                : "Shop has been rejected.";

            return ApiResponse<string>.Ok(message, message);
        }

        public async Task<ApiResponse<PagingResponse<GetShopResponse>>> GetShopsByStatusAsync(string status, PagingRequest request)
        {
            var totalCount = await _shopRepository.CountAsync(s => !s.IsDeleted && s.Status == status);

            var shops = await _shopRepository.FindWithIncludePagedAsync(
                predicate: s => !s.IsDeleted && s.Status == status,
                include: query => query
                    .Include(s => s.Owner)
                    .Include(s => s.ShopImages),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true);

            var shopResponses = shops.Select(s => new GetShopResponse
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                Address = s.Address,
                Status = s.Status,
                OpenHours = s.OpenHours,
                EndHours = s.EndHours,
                Rating = s.Rating,
                OwnerId = s.OwnerId,
                Owner = new GetUserResponse
                {
                    UserId = s.Owner.Id,
                    FullName = s.Owner.FullName,
                    UserName = s.Owner.UserName,
                    Phone = s.Owner.Phone,
                    Address = s.Owner.Address,
                    Avatar = s.Owner.Avatar,
                    Email = s.Owner.Email,
                    Dob = s.Owner.Dob,
                    RoleId = s.Owner.RoleId,
                    RoleName = s.Owner.Role?.Name ?? string.Empty
                },
                Images = new List<GetShopImageResponse>(s.ShopImages.Select(img => new GetShopImageResponse
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                }))
            }).ToList();

            if (!shopResponses.Any())
            {
                return ApiResponse<PagingResponse<GetShopResponse>>.Fail("Không tìm thấy cửa hàng nào.");
            }

            var pagingResponse = new PagingResponse<GetShopResponse>
            {
                Items = shopResponses,
                TotalItems = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return ApiResponse<PagingResponse<GetShopResponse>>.Ok(pagingResponse, "Lấy danh sách cửa hàng thành công.");
        }

        public async Task<ApiResponse<GetShopResponse>> GetShopByIdAsync(Guid shopId)
        {
            var shop = await _shopRepository.GetShopByIdAsync(shopId);

            if (shop == null)
            {
                return ApiResponse<GetShopResponse>.Fail("Không tìm thấy cửa hàng.");
            }

            var shopResponse = new GetShopResponse
            {
                Id = shop.Id,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Address = shop.Address,
                Status = shop.Status,
                OpenHours = shop.OpenHours,
                EndHours = shop.EndHours,
                Rating = shop.Rating,
                OwnerId = shop.OwnerId,
                Owner = new GetUserResponse
                {
                    UserId = shop.Owner.Id,
                    FullName = shop.Owner.FullName,
                    UserName = shop.Owner.UserName,
                    Phone = shop.Owner.Phone,
                    Address = shop.Owner.Address,
                    Avatar = shop.Owner.Avatar,
                    Email = shop.Owner.Email,
                    Dob = shop.Owner.Dob,
                    RoleId = shop.Owner.RoleId,
                    RoleName = shop.Owner.Role?.Name ?? string.Empty
                },
                Images = new List<GetShopImageResponse>(shop.ShopImages.Select(img => new GetShopImageResponse
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                }))
            };

            return ApiResponse<GetShopResponse>.Ok(shopResponse, "Lấy thông tin cửa hàng thành công.");
        }

        public async Task<ApiResponse<PagingResponse<GetShopResponse>>> GetShopsByOwnerIdAsync(Guid ownerId, PagingRequest request)
        {
            if (ownerId == Guid.Empty)
            {
                return ApiResponse<PagingResponse<GetShopResponse>>.Fail("Owner ID không hợp lệ.");
            }

            var totalCount = await _shopRepository.CountAsync(s => !s.IsDeleted && s.OwnerId == ownerId);

            var shops = await _shopRepository.FindWithIncludePagedAsync(
                predicate: s => !s.IsDeleted && s.OwnerId == ownerId,
                include: query => query
                    .Include(s => s.Owner)
                    .Include(s => s.ShopImages),
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                asNoTracking: true);

            var shopResponses = shops.Select(s => new GetShopResponse
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                Address = s.Address,
                Status = s.Status,
                OpenHours = s.OpenHours,
                EndHours = s.EndHours,
                Rating = s.Rating,
                OwnerId = s.OwnerId,
                Owner = new GetUserResponse
                {
                    UserId = s.Owner.Id,
                    FullName = s.Owner.FullName,
                    UserName = s.Owner.UserName,
                    Phone = s.Owner.Phone,
                    Address = s.Owner.Address,
                    Avatar = s.Owner.Avatar,
                    Email = s.Owner.Email,
                    Dob = s.Owner.Dob,
                    RoleId = s.Owner.RoleId,
                    RoleName = s.Owner.Role?.Name ?? string.Empty
                },
                Images = new List<GetShopImageResponse>(s.ShopImages.Select(img => new GetShopImageResponse
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                }))
            }).ToList();

            if (!shopResponses.Any())
            {
                return ApiResponse<PagingResponse<GetShopResponse>>.Fail("Không tìm thấy cửa hàng nào.");
            }

            var pagingResponse = new PagingResponse<GetShopResponse>
            {
                Items = shopResponses,
                TotalItems = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return ApiResponse<PagingResponse<GetShopResponse>>.Ok(pagingResponse, "Lấy danh sách cửa hàng thành công.");
        }

        public async Task<ApiResponse<GetShopResponse>> UpdateShopAsync(UpdateShopRequest request)
        {
            var shop = await _shopRepository.GetShopByIdAsync(request.ShopId);

            if (shop == null)
            {
                return ApiResponse<GetShopResponse>.Fail("Không tìm thấy cửa hàng.");
            }

            shop.Name = request.Name;
            shop.ImageUrl = request.ImageUrl;
            shop.Address = request.Address;
            shop.OpenHours = request.OpenHours;
            shop.EndHours = request.EndHours;

            await _shopRepository.UpdateAsync(shop);
            await _shopRepository.SaveChangesAsync();

            var shopResponse = new GetShopResponse
            {
                Id = shop.Id,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Address = shop.Address,
                Status = shop.Status,
                OpenHours = shop.OpenHours,
                EndHours = shop.EndHours,
                Rating = shop.Rating,
                OwnerId = shop.OwnerId,
                Owner = new GetUserResponse
                {
                    UserId = shop.Owner.Id,
                    FullName = shop.Owner.FullName,
                    UserName = shop.Owner.UserName,
                    Phone = shop.Owner.Phone,
                    Address = shop.Owner.Address,
                    Avatar = shop.Owner.Avatar,
                    Email = shop.Owner.Email,
                    Dob = shop.Owner.Dob,
                    RoleId = shop.Owner.RoleId,
                    RoleName = shop.Owner.Role?.Name ?? string.Empty
                },
                Images = new List<GetShopImageResponse>(shop.ShopImages.Select(img => new GetShopImageResponse
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                }))
            };

            return ApiResponse<GetShopResponse>.Ok(shopResponse, "Cập nhật cửa hàng thành công.");
        }

        public async Task<ApiResponse<string>> DeleteShopAsync(Guid shopId)
        {
            var shop = await _shopRepository.GetShopByIdAsync(shopId);

            if (shop == null)
            {
                return ApiResponse<string>.Fail("Không tìm thấy cửa hàng.");
            }

            await _shopRepository.SoftDeleteAsync(shop);
            await _shopRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("", "Cửa hàng đã được xóa thành công.");
        }

        public async Task<ApiResponse<string>> AddShopImageAsync(UpdateShopImageRequest request)
        {
            var shop = await _shopRepository.GetShopByIdAsync(request.ShopId);

            if (shop == null)
                return ApiResponse<string>.Fail("Không tìm thấy cửa hàng.");

            if (shop.ShopImages.Any(i => i.ImageUrl == request.ImageUrl))
            {
                return ApiResponse<string>.Fail("Ảnh đã tồn tại trong shop.");
            }

            var shopImage = new ShopImage
            {
                ShopId = request.ShopId,
                ImageUrl = request.ImageUrl
            };

            await _shopRepository.AddShopImageAsync(shopImage);
            await _shopRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("", "Thêm ảnh thành công.");
        }

    }
}
