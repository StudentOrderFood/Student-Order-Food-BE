using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.Models.Response.Shop;
using OrderFood_BE.Application.Models.Response.User;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Shop;
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
        public async Task<string> CreateShopAsync(CreateShopRequest request)
        {
            var user = await _userRepository.GetByIdAsync(request.OwnerId);
            if (user == null || user.Role.Name != RoleEnum.ShopOwner.ToString())
            {
                return "User is not authorized to create a shop.";
            }
            var shop = new Domain.Entities.Shop
            {
                Name = request.Name,
                //ImageUrl = request.ImageUrl,
                Address = request.Address,
                Status = ShopEnumStatus.Pending.ToString(), // Pending approval
                OpenHours = request.OpenHours,
                EndHours = request.EndHours,
                Rating = 0, // Default rating
                OwnerId = request.OwnerId
            };

            await _shopRepository.AddAsync(shop);
            await _shopRepository.SaveChangesAsync();
            return "The request has been submitted and is awaiting approval.";
        }

        public async Task<string> ApproveOrRejectShopAsync(ApproveShopRequest request)
        {
            var shop = await _shopRepository.GetShopByIdAsync(request.ShopId);
            if (shop == null)
                return "Shop not found.";

            if (shop.Status != ShopEnumStatus.Pending.ToString())
                return "This shop has already been processed.";

            shop.Status = request.IsApproved
                ? ShopEnumStatus.Approved.ToString()
                : ShopEnumStatus.Rejected.ToString();

            await _shopRepository.UpdateAsync(shop);
            await _shopRepository.SaveChangesAsync();

            return request.IsApproved ? "Shop approved successfully." : "Shop has been rejected.";
        }

        public async Task<PagingResponse<GetShopResponse>> GetShopsByStatusAsync(string status, PagingRequest request)
        {
            var totalCount = await _shopRepository.CountAsync(s => !s.IsDeleted && s.Status == status);

            var shops = await _shopRepository.FindWithIncludePagedAsync(
                predicate: s => !s.IsDeleted && s.Status == status,
                include: query => query
                    .Include(s => s.Owner),
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
                }
            }).ToList();

            if (!shopResponses.Any())
            {
                return new PagingResponse<GetShopResponse>
                {
                    Items = new List<GetShopResponse>(),
                    TotalItems = 0,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
            }

            var pagingResponse = new PagingResponse<GetShopResponse>
            {
                Items = shopResponses,
                TotalItems = totalCount,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize
            };

            return pagingResponse;
        }
    }
}
