using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Auth;
using OrderFood_BE.Application.UseCase.Interfaces.Auth;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationUseCase _authenticationUseCase;
        public AuthenticationsController(IAuthenticationUseCase authenticationUseCase)
        {
            _authenticationUseCase = authenticationUseCase;
        }
        /// <summary>
        /// Retrieves a new access token using a valid refresh token and user ID.
        /// </summary>
        /// <param name="request">The token request containing the user ID and refresh token.</param>
        /// <returns>A <see cref="TokenResponse"/> containing a new access token, the same refresh token, user ID, and user role.</returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> GetNewAccessToken([FromBody] TokenRequest request)
        {
            var response = await _authenticationUseCase.GetNewAccessToken(request);
            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                return BadRequest("Invalid refresh token or user not found.");
            }
            return Ok(response);
        }
        [HttpPost("student-login")]
        public async Task<IActionResult> StudentLogin([FromBody] IdTokenRequest request)
        {
            var response = await _authenticationUseCase.StudentLoginAsync(request);
            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                return BadRequest("Invalid email or password.");
            }
            return Ok(response);
        }
    }
}
