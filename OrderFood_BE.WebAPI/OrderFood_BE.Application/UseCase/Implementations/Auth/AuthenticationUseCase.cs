using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using OrderFood_BE.Application.Models.Requests.Auth;
using OrderFood_BE.Application.Models.Response.Auth;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Application.UseCase.Interfaces.Auth;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Shared.Enums;

namespace OrderFood_BE.Application.UseCase.Implementations.Auth
{
    public class AuthenticationUseCase : IAuthenticationUseCase
    {
        private readonly FirebaseApp _firebaseApp;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRoleRepository _roleRepository;
        public AuthenticationUseCase(IUserRepository userRepository, IJwtService jwtService, IRoleRepository roleRepository)
        {
            // Initialize FirebaseApp if it is not already initialized
            if (FirebaseApp.DefaultInstance == null)
            {
                _firebaseApp = FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("D:\\student-order-food-firebase-adminsdk-fbsvc-e0b64bf4df.json")
                });
            }
            else
            {
                _firebaseApp = FirebaseApp.DefaultInstance;
            }
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
        public async Task<TokenResponse> GetNewAccessToken(TokenRequest request)
        {
            // Validate the refresh token and user ID
            var isValid = await _jwtService.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);
            // Nếu invalid return TokenResponse rỗng
            if (!isValid)
            {
                Console.WriteLine("Invalid refresh token");
                return new TokenResponse();
            }
            //Ngược lại
            // Lấy thông tin người dùng từ DB bằng UserId
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                Console.WriteLine("User not found");
                return new TokenResponse();
            }
            // Tạo một access token mới
            var responseToken = new TokenResponse
            {
                AccessToken = _jwtService.GenerateAccessToken(),
                RefreshToken = request.RefreshToken,
                UserId = user.Id.ToString(),
                UserRole = user.Role.Name
            };

            // Trả về TokenResponse chứa access token, refresh token, userId và userRole
            return responseToken;
        }

        /// <summary>
        /// Authenticates a student using a Firebase ID token. If the user does not exist, creates a new user.
        /// </summary>
        /// <param name="idToken">The Firebase ID token.</param>
        /// <returns>A <see cref="TokenResponse"/> containing access and refresh tokens, user ID, and role.</returns>
        public async Task<TokenResponse> StudentLoginAsync(string idToken)
        {
            try
            {
                // Lấy FirebaseAuth từ FirebaseApp
                var auth = FirebaseAuth.GetAuth(_firebaseApp);

                // Xác thực ID token và uid
                var decodedToken = await auth.VerifyIdTokenAsync(idToken);
                var uid = decodedToken.Uid;
                //lấy thông tin user từ Firebase bằng UID
                var user = await auth.GetUserAsync(uid);
                /**
                 * Kiểm tra xem người dùng có tồn tại trong DB không?
                 * Nếu chưa, Tạo mới vào DB và return về user
                 * Nếu rồi, return về user đã có trong DB
                 */
                // Case 1: Người dùng đã tồn tại trong DB => Trả về TokenReponse chứa thông tin người dùng
                if (await _userRepository.ExistsByEmailAsync(user.Email))
                {
                    // Tạo JWT
                    var accessToken = _jwtService.GenerateAccessToken();
                    var refreshToken = await _jwtService.GenerateRefreshTokenAsync();
                    // Lấy thông tin người dùng từ DB
                    var existingUser = await _userRepository.GetByEmailAsync(user.Email);
                    return new TokenResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        UserId = existingUser.Id.ToString(),
                        UserRole = existingUser.Role.Name,
                    };

                }
                // Case 2: Người dùng chưa tồn tại trong DB => Tạo mới vào DB và trả về TokenResponse
                else
                {
                    var role = await _roleRepository.GetByNameAsync(RoleEnum.Student.ToString());
                    if (role == null)
                    {
                        Console.WriteLine("Cannot find role Student");
                        return new TokenResponse();
                    }
                    var User = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = user.Email,
                        FullName = user.DisplayName,
                        Phone = user.PhoneNumber,
                        RoleId = role.Id,
                        Avatar = user.PhotoUrl,
                    };
                    // Lưu người dùng vào DB
                    await _userRepository.AddAsync(User);
                    await _userRepository.SaveChangesAsync();
                    // Tạo JWT
                    var accessToken = _jwtService.GenerateAccessToken();
                    var refreshToken = await _jwtService.GenerateRefreshTokenAsync();
                    return new TokenResponse
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        UserId = User.Id.ToString(),
                        UserRole = User.Role.Name,
                    };
                }
            }
            catch (FirebaseAuthException ex)
            {
                // Xử lý lỗi nếu có
                Console.WriteLine($"FirebaseAuthException: {ex.Message}");
                return new TokenResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                return new TokenResponse();
            }
        }
    }
}
