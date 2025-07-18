using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.User;
using OrderFood_BE.Application.Models.Response.User;
using OrderFood_BE.Application.UseCase.Interfaces.User;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;

        public UsersController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin,ShopOwner,Student")]
        [ProducesResponseType(typeof(ApiResponse<GetUserResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<GetUserResponse>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetByIdAsync(Guid userId)
        {
            var user = await _userUseCase.GetByIdAsync(userId);
            return Ok(user);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userUseCase.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("shop-owners")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllShopOwnersAsync()
        {
            var shopOwners = await _userUseCase.GetAllShopOwnerAsync();
            return Ok(shopOwners);
        }

        [HttpGet("students")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserResponse>>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await _userUseCase.GetAllStudentAsync();
            return Ok(students);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,ShopOwner,Student")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateRequest request)
        {
            var profile = await _userUseCase.UpdateProfileAsync(request);
            return Ok(profile);
        }

        [HttpGet("checkPhoneNumberExists")]
        [Authorize(Roles = "Admin,ShopOwner,Student")] // nếu bạn cần xác thực
        public async Task<IActionResult> CheckPhoneNumberExists([FromQuery] string phoneNumber)
        {
            var result = await _userUseCase.CheckPhoneNumberExists(phoneNumber);
            return Ok(result);
        }
    }
}
