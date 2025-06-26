using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Requests.Auth;
using OrderFood_BE.Application.Models.Response.Auth;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Interfaces.Auth
{
    public interface IAuthenticationUseCase
    {
        /// <summary>
        /// Authenticates a student using a Firebase ID token. If the user does not exist, creates a new user.
        /// </summary>
        /// <param name="idToken">The Firebase ID token.</param>
        /// <returns>A <see cref="TokenResponse"/> containing access and refresh tokens, user ID, and role.</returns>
        Task<ApiResponse<TokenResponse>> StudentLoginAsync(IdTokenRequest request);
        /// <summary>
        /// Retrieves a new access token using a valid refresh token and user ID.
        /// </summary>
        /// <param name="request">The token request containing the user ID and refresh token.</param>
        /// <returns>A <see cref="TokenResponse"/> containing a new access token, the same refresh token, user ID, and user role.</returns>
        Task<TokenResponse> GetNewAccessToken(TokenRequest request);
        /// <summary>
        /// Registers a new shop owner account in the system.
        /// </summary>
        /// <param name="request">The registration request containing user details such as full name, username, password, phone, address, avatar, email, and date of birth.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains a string message indicating the result of the registration:
        /// <list type="bullet">
        /// <item><description>"Register account successfully." if registration is successful.</description></item>
        /// <item><description>"Role does not exists." if the ShopOwner role is not found.</description></item>
        /// <item><description>"UserName or Email or Phone already exists." if the username, email, or phone already exists in the database.</description></item>
        /// <item><description>"Confirm password does not match." if the password and confirm password do not match.</description></item>
        /// </list>
        /// </returns>
        Task<string> RegisterShopOwnerAsync(RegisterRequest request);
        /// <summary>
        /// Authenticates a user using their identifier (email, phone, or username) and password.
        /// </summary>
        /// <param name="request">The login request containing the user's identifier and password.</param>
        /// <returns>
        /// A <see cref="TokenResponse"/> containing access and refresh tokens, user ID, and user role if authentication is successful;
        /// otherwise, an empty <see cref="TokenResponse"/> if authentication fails.
        /// </returns>
        Task<TokenResponse> LoginAsync(LoginRequest request);
    }
}
