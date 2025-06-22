using OrderFood_BE.Application.Models.Response.User;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.User;
using OrderFood_BE.Shared.Enums;

namespace OrderFood_BE.Application.UseCase.Implementations.User
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;
        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<GetUserResponse>?> GetAllAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            if (users == null || !users.Any())
            {
                return null;
            }
            return users
                .Select(u => new GetUserResponse
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Phone = u.Phone,
                    Address = u.Address,
                    Avatar = u.Avatar,
                    Email = u.Email,
                    Dob = u.Dob,
                    RoleId = u.RoleId,
                    RoleName = u.Role?.Name ?? string.Empty
                }).ToList();
        }
        public async Task<List<GetUserResponse>?> GetAllShopOwnerAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            if (users == null || !users.Any())
            {
                return null;
            }
            return users
                .Where(u => u.Role.Name == RoleEnum.ShopOwner.ToString())
                .Select(u => new GetUserResponse
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Phone = u.Phone,
                    Address = u.Address,
                    Avatar = u.Avatar,
                    Email = u.Email,
                    Dob = u.Dob,
                    RoleId = u.RoleId,
                    RoleName = u.Role?.Name ?? string.Empty
                }).ToList();
        }
        public async Task<List<GetUserResponse>?> GetAllStudentAsync()
        {
            var users = await _userRepository.GetAllUserAsync();
            if (users == null || !users.Any())
            {
                return null;
            }
            return users
                .Where(u => u.Role.Name == RoleEnum.Student.ToString())
                .Select(u => new GetUserResponse
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    UserName = u.UserName,
                    Phone = u.Phone,
                    Address = u.Address,
                    Avatar = u.Avatar,
                    Email = u.Email,
                    Dob = u.Dob,
                    RoleId = u.RoleId,
                    RoleName = u.Role?.Name ?? string.Empty
                }).ToList();
        }
        public async Task<GetUserResponse?> GetByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return null;
            }
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null || user.IsDeleted || user.IsActive == false)
            {
                return null;
            }
            return new GetUserResponse
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
            };
        }
    }
}
