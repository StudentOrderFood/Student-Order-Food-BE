using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Auth;
using OrderFood_BE.Application.UseCase.Interfaces.Auth;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationUseCase _authenticationUseCase;
        public AuthenticationsController(IAuthenticationUseCase authenticationUseCase)
        {
            _authenticationUseCase = authenticationUseCase;
        }
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
                return BadRequest("Invalid email");
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authenticationUseCase.LoginAsync(request);
            if (response == null || string.IsNullOrEmpty(response.AccessToken))
            {
                return Ok("Invalid identifier or password..");
            }
            return Ok(response);
        }

        [HttpPost("register-shop-owner")]
        public async Task<IActionResult> RegisterShopOwner([FromBody] RegisterRequest request)
        {
            var response = await _authenticationUseCase.RegisterShopOwnerAsync(request);
            if (string.IsNullOrEmpty(response))
            {
                return BadRequest("Registration failed.");
            }
            if (response == "UserName or Email or Phone already exists.")
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
