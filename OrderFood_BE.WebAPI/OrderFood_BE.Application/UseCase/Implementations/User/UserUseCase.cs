using OrderFood_BE.Application.Models.Requests.User;
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

        public async Task<ApiResponse<string>> CheckPhoneNumberExists(string phoneNumber)
        {
            var checkItem = await _userRepository.ExistsAsync(phoneNumber);
            if (checkItem)
            {
                return ApiResponse<string>.Ok("Phone number already exists", "Phone number already exists");
            }
            else
            {
                return ApiResponse<string>.Ok("Phone number does not exist", "Phone number does not exist");
            }

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
                    WalletBalance = u.WalletBalance,
                    RoleId = u.RoleId,
                    RoleName = u.Role?.Name ?? string.Empty
                })
                .ToList();

            return ApiResponse<List<GetUserResponse>>.Ok(response, "Lấy danh sách người dùng thành công.");
        }

        public async Task<ApiResponse<List<GetCustomerResponse>>> GetAllCustomerAsync()
        {
            var customers = await _userRepository.GetAllUserAsync();
            if (customers == null || !customers.Any())
            {
                return ApiResponse<List<GetCustomerResponse>>.Fail("Không tìm thấy khách hàng nào.");
            }
            var response = customers
                .Where(u => u.Role.Name == RoleEnum.Student.ToString())
                .Select(u => new GetCustomerResponse
                {
                    UserId = u.Id,
                    FullName = u.FullName,
                    Phone = u.Phone,
                    Address = u.Address
                }).ToList();

            return ApiResponse<List<GetCustomerResponse>>.Ok(response, "Lấy danh sách khách hàng thành công.");

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
                    WalletBalance = u.WalletBalance,
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
                    WalletBalance = u.WalletBalance,
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
                WalletBalance = user.WalletBalance,
                RoleId = user.RoleId,
                RoleName = user.Role?.Name ?? string.Empty
            };

            return ApiResponse<GetUserResponse>.Ok(response, "Lấy thông tin người dùng thành công.");
        }

        public async Task<ApiResponse<string>> UpdateProfileAsync(ProfileUpdateRequest request)
        {
            if (request == null)
            {
                return ApiResponse<string>.Fail("Yêu cầu cập nhật không hợp lệ.");
            }

            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null || user.IsDeleted || !user.IsActive)
            {
                return ApiResponse<string>.Fail("Người dùng không tồn tại hoặc đã bị xóa hoặc không hoạt động.");
            }

            // Update user properties
            user.FullName = request.FullName;
            user.Phone = request.Phone;
            user.Address = request.Address;
            user.Dob = request.Dob;
            user.Avatar = request.Avatar;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok("Cập nhật thông tin người dùng thành công.", "Cập nhật thông tin người dùng thành công.");
        }

        public async Task<bool> UpdateUserWallet(Guid shopId, decimal amount)
        {
            if (shopId == Guid.Empty || amount <= 0)
            {
                return false; // Invalid parameters
            }
            var user = await _userRepository.GetUserByShopId(shopId);
            if (user == null || user.IsDeleted || !user.IsActive)
            {
                return false; // User not found or inactive
            }
            user.WalletBalance += amount;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return true; // Update successful
        }
    }
}
