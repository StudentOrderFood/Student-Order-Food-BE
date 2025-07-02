using System.Text.Json;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using OrderFood_BE.Application.Models.Requests.Auth;
using OrderFood_BE.Application.Models.Response.Auth;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Application.UseCase.Interfaces.Auth;
using OrderFood_BE.Shared.Common;
using OrderFood_BE.Shared.Enums;

namespace OrderFood_BE.Application.UseCase.Implementations.Auth
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        //private readonly FirebaseApp _firebaseApp;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRoleRepository _roleRepository;
        public AuthenticationUseCase(IUserRepository userRepository, IJwtService jwtService, IRoleRepository roleRepository)
        {
            //// Initialize FirebaseApp if it is not already initialized
            //if (FirebaseApp.DefaultInstance == null)
            //{
            //    _firebaseApp = FirebaseApp.Create(new AppOptions()
            //    {
            //        Credential = GoogleCredential.FromFile("D:\\student-order-food-firebase-adminsdk-fbsvc-e0b64bf4df.json")
            //    });
            //}
            //else
            //{
            //    _firebaseApp = FirebaseApp.DefaultInstance;
            //}
            _userRepository = userRepository;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Retrieves a new access token using a valid refresh token and user ID.
        /// </summary>
        /// <param name="request">The token request containing the user ID and refresh token.</param>
        /// <returns>
        /// A <see cref="TokenResponse"/> containing a new access token, the same refresh token, user ID, and user role.
        /// Returns an empty <see cref="TokenResponse"/> if the refresh token is invalid or the user is not found.
        /// </returns>
        public async Task<ApiResponse<TokenResponse>> GetNewAccessToken(TokenRequest request)
        {
            // Validate the refresh token and user ID
            var isValid = await _jwtService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            // Không có refreshToken hoặc refreshToken hết hạn
            if (!isValid)
            {
                //Console.WriteLine("Invalid refresh token");
                return ApiResponse<TokenResponse>.Fail("Invalid refresh token");
            }
            //Ngược lại
            // Lấy thông tin người dùng từ DB bằng UserId
            var user = await _userRepository.GetUserByIdAsync(request.UserId);
            if (user == null)
            {
                //Console.WriteLine("User not found");
                return ApiResponse<TokenResponse>.Fail("User not found");
            }
            // Tạo một access token mới
            var responseToken = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Role.Name, user.Id),
                RefreshToken = request.RefreshToken,
                UserId = user.Id.ToString(),
                UserRole = user.Role.Name
            };
            return ApiResponse<TokenResponse>.Ok(responseToken, "Refresh successfully");
        }
        public async Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request)
        {
            var user = await _userRepository.GetUserByEmailPhoneOrUserName(request.Identifier);
            if (user == null)
            {
                //return new TokenResponse { };
                return ApiResponse<TokenResponse>.Fail("Invalid identifier or passwords");
            }
            // Kiểm tra mật khẩu
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!isPasswordValid)
            {
                //return new TokenResponse { };
                return ApiResponse<TokenResponse>.Fail("Invalid identifire or passwords");
            }

            if (user.IsActive == false )
            {
                return ApiResponse<TokenResponse>.Fail("Need to wait for admin to approve");
            }
            // Tạo JWT
            var tokenReponse = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(user.Role.Name, user.Id),
                RefreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id),
                UserId = user.Id.ToString(),
                UserRole = user.Role.Name,
            };
            //return tokenReponse;
            return ApiResponse<TokenResponse>.Ok(tokenReponse, "Login Succesfully");
        }
        public async Task<ApiResponse<string>> RegisterShopOwnerAsync(RegisterRequest request)
        {
            var role = await _roleRepository.GetByNameAsync(RoleEnum.ShopOwner.ToString());
            if (role == null)
            {
                //return "Role does not exists.";
                return ApiResponse<string>.Fail("Role does not exists.");
            }
            // Kiểm tra xem email or username or phone người dùng đã tồn tại trong DB chưa
            var userExists = await _userRepository.ExistsAsync(request.Email) || await _userRepository.ExistsAsync(request.UserName) || await _userRepository.ExistsAsync(request.Phone);
            if (userExists)
            {
                //return "UserName or Email or Phone already exists.";
                return ApiResponse<string>.Fail("UserName or Email or Phone already exists.");
            }
            // Kiểm tra confirm password
            if (request.Password != request.ConfirmPassword)
            {
                //return "Confirm password does not match.";
                return ApiResponse<string>.Fail("Confirm password does not match.");
            }
            // Mã hóa mật khẩu
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
            // Tạo người dùng mới
            var user = new Domain.Entities.User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FullName = request.FullName,
                UserName = request.UserName,
                Password = hashedPassword,
                Phone = request.Phone,
                Address = request.Address,
                Avatar = request.Avatar ?? "",
                RoleId = role.Id,
                Dob = request.Dob,
                IsActive = true,
            };

            // Lưu người dùng vào DB
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            // return success message
            //return "Register account successfully.";
            return ApiResponse<string>.Ok("Register account successfully.", "Register account successfully.");
        }
        //public async Task<ApiResponse<TokenResponse>> StudentLoginAsync(IdTokenRequest request)
        //{
        //    try
        //    {
        //        // Lấy FirebaseAuth từ FirebaseApp
        //        var auth = FirebaseAuth.GetAuth(_firebaseApp);

        //        // Xác thực ID token và uid
        //        var decodedToken = await auth.VerifyIdTokenAsync(request.IdToken);
        //        var uid = decodedToken.Uid;
        //        //lấy thông tin user từ Firebase bằng UID
        //        var user = await auth.GetUserAsync(uid);
        //        if (string.IsNullOrEmpty(user.Email) || !user.Email.EndsWith(".edu.vn", StringComparison.OrdinalIgnoreCase))
        //        {
        //            // Email is not a valid student email
        //            return new ApiResponse<TokenResponse>
        //            {
        //                Data = null,
        //                Success = false,
        //                Message = "Email must end with .edu.vn"
        //            };
        //        }
        //        /**
        //         * Kiểm tra xem người dùng có tồn tại trong DB không?
        //         * Nếu chưa, Tạo mới vào DB và return về user
        //         * Nếu rồi, return về user đã có trong DB
        //         */
        //        // Case 1: Người dùng đã tồn tại trong DB => Trả về TokenReponse chứa thông tin người dùng
        //        if (await _userRepository.ExistsByEmailAsync(user.Email))
        //        {
        //            // Lấy thông tin người dùng từ DB
        //            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        //            // Tạo JWT
        //            var accessToken = _jwtService.GenerateAccessToken(existingUser.Role.Name);
        //            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(existingUser.Id);
        //            var respose =  new TokenResponse
        //            {
        //                AccessToken = accessToken,
        //                RefreshToken = refreshToken,
        //                UserId = existingUser.Id.ToString(),
        //                UserRole = existingUser.Role.Name,
        //            };
        //            return new ApiResponse<TokenResponse>
        //            {
        //                Data = respose,
        //                Success = true,
        //                Message = "Login successfully"
        //            };

        //        }
        //        // Case 2: Người dùng chưa tồn tại trong DB => Tạo mới vào DB và trả về TokenResponse
        //        else
        //        {
        //            var role = await _roleRepository.GetByNameAsync(RoleEnum.Student.ToString());
        //            if (role == null)
        //            {
        //                return new ApiResponse<TokenResponse>
        //                {
        //                    Data = null,
        //                    Success = false,
        //                    Message = "Role not valid"
        //                };
        //            }
        //            var User = new Domain.Entities.User
        //            {
        //                Id = Guid.NewGuid(),
        //                Email = user.Email,
        //                FullName = user.DisplayName,
        //                Phone = user.PhoneNumber ?? "",
        //                RoleId = role.Id,
        //                Avatar = user.PhotoUrl,
        //                IsActive = true
        //            };
        //            // Lưu người dùng vào DB
        //            await _userRepository.AddAsync(User);
        //            await _userRepository.SaveChangesAsync();
        //            // Tạo JWT
        //            var accessToken = _jwtService.GenerateAccessToken(role.Name);
        //            var refreshToken = await _jwtService.GenerateRefreshTokenAsync(User.Id);
        //            var repsponse =  new TokenResponse
        //            {
        //                AccessToken = accessToken,
        //                RefreshToken = refreshToken,
        //                UserId = User.Id.ToString(),
        //                UserRole = User.Role.Name,
        //            };
        //            return new ApiResponse<TokenResponse>
        //            {
        //                Data = repsponse,
        //                Success = true,
        //                Message = "Login successfully"
        //            };
        //        }
        //    }
        //    catch (FirebaseAuthException ex)
        //    {
        //        // Xử lý lỗi nếu có
        //        Console.WriteLine($"FirebaseAuthException: {ex.Message}");
        //        //return new TokenResponse();
        //        return new ApiResponse<TokenResponse>
        //        {
        //            Data = null,
        //            Success = false,
        //            Message = "Firebase Authentication Exception"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"General Exception: {ex.Message}");
        //        return new ApiResponse<TokenResponse>
        //        {
        //            Data = null,
        //            Success = false,
        //            Message = "Unknown exception"
        //        };
        //    }
        //}

        public async Task<ApiResponse<TokenResponse>> StudentLoginAsync(IdTokenRequest request)
        {
            try
            {
                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}");

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Invalid Google ID Token"
                    };
                }

                var content = await response.Content.ReadAsStringAsync();
                var tokenInfo = JsonSerializer.Deserialize<GoogleTokenInfo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (tokenInfo == null || tokenInfo.EmailVerified != "true" || !tokenInfo.Email.EndsWith(".edu.vn", StringComparison.OrdinalIgnoreCase))
                {
                    return new ApiResponse<TokenResponse>
                    {
                        Data = null,
                        Success = false,
                        Message = "Email must be verified and end with .edu.vn"
                    };
                }

                var email = tokenInfo.Email;
                var name = tokenInfo.Name;
                var avatar = tokenInfo.Picture;

                if (await _userRepository.ExistsByEmailAsync(email))
                {
                    var existingUser = await _userRepository.GetByEmailAsync(email);
                    var accessToken = _jwtService.GenerateAccessToken(existingUser.Role.Name, existingUser.Id);
                    var refreshToken = await _jwtService.GenerateRefreshTokenAsync(existingUser.Id);
                    return new ApiResponse<TokenResponse>
                    {
                        Data = new TokenResponse
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            UserId = existingUser.Id.ToString(),
                            UserRole = existingUser.Role.Name
                        },
                        Success = true,
                        Message = "Login successfully"
                    };
                }
                else
                {
                    var role = await _roleRepository.GetByNameAsync(RoleEnum.Student.ToString());
                    if (role == null)
                    {
                        return new ApiResponse<TokenResponse>
                        {
                            Data = null,
                            Success = false,
                            Message = "Role not valid"
                        };
                    }
                    var user = new Domain.Entities.User
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        FullName = name,
                        Phone = string.Empty,
                        RoleId = role.Id,
                        Avatar = avatar,
                        IsActive = true
                    };
                    await _userRepository.AddAsync(user);
                    await _userRepository.SaveChangesAsync();

                    var accessToken = _jwtService.GenerateAccessToken(role.Name, user.Id);
                    var refreshToken = await _jwtService.GenerateRefreshTokenAsync(user.Id);
                    return new ApiResponse<TokenResponse>
                    {
                        Data = new TokenResponse
                        {
                            AccessToken = accessToken,
                            RefreshToken = refreshToken,
                            UserId = user.Id.ToString(),
                            UserRole = role.Name
                        },
                        Success = true,
                        Message = "Login successfully"
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new ApiResponse<TokenResponse>
                {
                    Data = null,
                    Success = false,
                    Message = "Login failed"
                };
            }
        }
    }
}
