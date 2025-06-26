using OrderFood_BE.Application.Models.Response.User;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.User;
using OrderFood_BE.Shared.Common;
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
        public async Task<ApiResponse<List<GetUserResponse>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllUserAsync();

            if (users == null || !users.Any())
            {
                return ApiResponse<List<GetUserResponse>>.Fail("Không tìm thấy người dùng nào.");
            }

            var response = users
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
                })
                .ToList();

            return ApiResponse<List<GetUserResponse>>.Ok(response, "Lấy danh sách người dùng thành công.");
        }

        public async Task<ApiResponse<List<GetUserResponse>>> GetAllShopOwnerAsync()
        {
            var users = await _userRepository.GetAllUserAsync();

            if (users == null || !users.Any())
            {
                return ApiResponse<List<GetUserResponse>>.Fail("Không tìm thấy chủ cửa hàng nào.");
            }

            var response = users
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

            return ApiResponse<List<GetUserResponse>>.Ok(response, "Lấy danh sách chủ cửa hàng thành công.");
        }

        public async Task<ApiResponse<List<GetUserResponse>>> GetAllStudentAsync()
        {
            var users = await _userRepository.GetAllUserAsync();

            if (users == null || !users.Any())
            {
                return ApiResponse<List<GetUserResponse>>.Fail("Không tìm thấy học sinh nào.");
            }

            var response = users
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

            return ApiResponse<List<GetUserResponse>>.Ok(response, "Lấy danh sách học sinh thành công.");
        }

        public async Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return ApiResponse<GetUserResponse>.Fail("Mã người dùng không hợp lệ.");
            }

            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null || user.IsDeleted || user.IsActive == false)
            {
                return ApiResponse<GetUserResponse>.Fail("Người dùng không tồn tại hoặc đã bị xóa hoặc không hoạt động.");
            }

            var response = new GetUserResponse
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

            return ApiResponse<GetUserResponse>.Ok(response, "Lấy thông tin người dùng thành công.");
        }
    }
}
