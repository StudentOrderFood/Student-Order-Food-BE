using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.UseCase.Interfaces.User;
using OrderFood_BE.Shared.Enums;

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
        public async Task<IActionResult> GetByIdAsync(Guid userId)
        {
            var user = await _userUseCase.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAsync()
        {
            var users = await _userUseCase.GetAllAsync();
            if (users == null || !users.Any())
            {
                return NotFound("No users found");
            }
            return Ok(users);
        }
        [HttpGet("shop-owners")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllShopOwnersAsync()
        {
            var shopOwners = await _userUseCase.GetAllShopOwnerAsync();
            if (shopOwners == null || !shopOwners.Any())
            {
                return NotFound("No shop owners found");
            }
            return Ok(shopOwners);
        }
        [HttpGet("students")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStudentsAsync()
        {
            var students = await _userUseCase.GetAllStudentAsync();
            if (students == null || !students.Any())
            {
                return NotFound("No students found");
            }
            return Ok(students);
        }
    }
}
