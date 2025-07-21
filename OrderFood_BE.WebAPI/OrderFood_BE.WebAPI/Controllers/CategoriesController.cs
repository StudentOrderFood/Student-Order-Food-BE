using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Category;
using OrderFood_BE.Application.Models.Response.Category;
using OrderFood_BE.Application.UseCase.Interfaces.Category;
using OrderFood_BE.Shared.Common;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryUseCase _categoryUseCase;

        public CategoriesController(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<GetCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var result = await _categoryUseCase.CreateCategoryAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<GetCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var result = await _categoryUseCase.UpdateCategoryAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryUseCase.DeleteCategoryAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<GetCategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryUseCase.GetCategoryByIdAsync(id);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetCategoryResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryUseCase.GetAllCategoriesAsync();
            return Ok(result);
        }
    }
}
