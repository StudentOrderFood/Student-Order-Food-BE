using OrderFood_BE.Application.Models.Requests.Shop;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Shop;
using OrderFood_BE.Shared.Enums;

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
                IsActive = false, // Pending approval
                OpenHours = request.OpenHours,
                EndHours = request.EndHours,
                Rating = 0, // Default rating
                OwnerId = request.OwnerId
            };

            await _shopRepository.AddAsync(shop);
            await _shopRepository.SaveChangesAsync();
            return "The request has been submitted and is awaiting approval.";
        }
    }
}
