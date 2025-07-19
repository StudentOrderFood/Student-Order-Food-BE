using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.Models.Response.Category;
using OrderFood_BE.Application.Models.Response.MenuItem;
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

            if (user != null) //&& user.Role.Name == RoleEnum.ShopOwner.ToString()
            {
                var shop = new Domain.Entities.Shop
                {
                    Name = request.Name,
                    ImageUrl = !string.IsNullOrEmpty(request.ImageUrl) ? request.ImageUrl : null,
                    Address = request.Address,
                    Status = ShopEnumStatus.Pending.ToString(), // Pending approval
                    OpenHours = request.OpenHours,
                    EndHours = request.EndHours,
                    Rating = 0, // Default rating
                    BusinessLicenseImageUrl = !string.IsNullOrEmpty(request.BusinessLicenseImageUrl) ? request.BusinessLicenseImageUrl : null,
                    Latitude = request.Latitude,
                    Longitude = request.Longitude,
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
                    BusinessLicenseImageUrl = shop.BusinessLicenseImageUrl,
                    Note = shop.Note,
                    Latitude = shop.Latitude,
                    Longitude = shop.Longitude,
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

            return ApiResponse<GetShopResponse>.Fail("User is not authorized to create a shop.");
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

            if (!string.IsNullOrEmpty(request.Note))
            {
                shop.Note = request.Note;
            }

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
                BusinessLicenseImageUrl = s.BusinessLicenseImageUrl,
                Note = s.Note,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
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
                BusinessLicenseImageUrl = shop.BusinessLicenseImageUrl,
                Note = shop.Note,
                Latitude = shop.Latitude,
                Longitude = shop.Longitude,
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
                BusinessLicenseImageUrl = s.BusinessLicenseImageUrl,
                Note = s.Note,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
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

            if (shop.OwnerId != request.OwnerId)
            {
                return ApiResponse<GetShopResponse>.Fail("Bạn không có quyền cập nhật cửa hàng này.");
            }

            shop.Name = request.Name;
            if (!string.IsNullOrEmpty(request.ImageUrl))
            {
                shop.ImageUrl = request.ImageUrl;
            }
            shop.Address = request.Address;
            shop.OpenHours = request.OpenHours;
            shop.EndHours = request.EndHours;
            if (!string.IsNullOrEmpty(request.BusinessLicenseImageUrl))
            {
                shop.BusinessLicenseImageUrl = request.BusinessLicenseImageUrl;
            }
            shop.Latitude = request.Latitude;
            shop.Longitude = request.Longitude;

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
                BusinessLicenseImageUrl = shop.BusinessLicenseImageUrl,
                Note = shop.Note,
                Latitude = shop.Latitude,
                Longitude = shop.Longitude,
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

        public async Task<ApiResponse<GetShopDetailResponse>> GetShopIncludeItemsAndCategoryByIdAsync(Guid shopId)
        {
            var shop = await _shopRepository.GetShopDetailByIdAsync(shopId, includeMenuItems: true, includeCategoryItems: true);
            if (shop == null)
            {
                return ApiResponse<GetShopDetailResponse>.Fail("Cannot find the shop match with Id");
            }
            // Ensure that menu items are not deleted and are available
            var validMenuItems = shop.MenuItems
                .Where(mi => !mi.IsDeleted && mi.IsAvailable)
                .ToList();

            var distinctCategories = validMenuItems
                .Where(mi => mi.Category != null)
                .Select(mi => new GetCategoriesInShopMenu
                {
                    Id = mi.Category.Id,
                    Name = mi.Category.Name
                })
                .DistinctBy(c => c.Id)
                .ToList();

            var response = new GetShopDetailResponse
            {
                Id = shop.Id,
                Name = shop.Name,
                ImageUrl = shop.ImageUrl,
                Address = shop.Address,
                Status = shop.Status,
                OpenHours = shop.OpenHours,
                EndHours = shop.EndHours,
                Rating = shop.Rating,
                Latitude = shop.Latitude,
                Longitude = shop.Longitude,
                Images = new List<GetShopImageResponse>(shop.ShopImages.Select(img => new GetShopImageResponse
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl
                })),
                Categories = distinctCategories,
                MenuItems = validMenuItems
                    .Select(mi => new GetMenuItemResponse
                    {
                        Id = mi.Id,
                        Name = mi.Name,
                        Description = mi.Description,
                        Price = mi.Price,
                        ImageUrl = mi.ImageUrl,
                        CategoryId = mi.CategoryId,
                    }).ToList()
            };

            return ApiResponse<GetShopDetailResponse>.Ok(response, "Get Shop Detail Info Success");
        }

        public async Task<ApiResponse<List<GetPopularShopResponse>>> GetPopularShopAsync(string currentTime)
        {
            if (!TimeSpan.TryParse(currentTime, out var currentTimeSpan))
                return ApiResponse<List<GetPopularShopResponse>>.Fail("Invalid time format.");

            var mealType = TimeUtils.DetermineMeal(currentTimeSpan); // Dinner, Lunch, Breakfast

            var popularShops = await _shopRepository.GetPopularShopsByTimeAndMealAsync(currentTimeSpan, mealType);
            if (popularShops == null || !popularShops.Any())
            {
                return ApiResponse<List<GetPopularShopResponse>>.Fail("No popular shops found for the specified time.");
            }
            var shopResponses = popularShops.Select(s => new GetPopularShopResponse
            {
                Id = s.Id,
                Name = s.Name,
                ImageUrl = s.ImageUrl,
                Address = s.Address,
                Status = s.Status,
                OpenHours = s.OpenHours,
                EndHours = s.EndHours,
                Rating = s.Rating,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                CategoryIds = s.MenuItems
                    .Where(mi => !mi.IsDeleted && mi.IsAvailable)
                    .Select(mi => mi.CategoryId)
                    .Distinct()
                    .ToList()
            }).ToList();
            return ApiResponse<List<GetPopularShopResponse>>.Ok(shopResponses, "Get Popular Shops Success");
        }
    }
}
