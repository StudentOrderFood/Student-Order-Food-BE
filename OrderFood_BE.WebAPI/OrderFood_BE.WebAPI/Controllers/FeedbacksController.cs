using CloudinaryDotNet;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Feedback;
using OrderFood_BE.Application.Models.Response.Feedback;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Application.UseCase.Interfaces.Feedback;
using OrderFood_BE.Shared.Common;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackUseCase _feedbackUseCase;
        private readonly ICloudinaryService _cloudinaryService;

        public FeedbacksController(IFeedbackUseCase feedbackUseCase, ICloudinaryService cloudinaryService)
        {
            _feedbackUseCase = feedbackUseCase;
            _cloudinaryService = cloudinaryService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetFeedbackResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateFeedbackAsync([FromForm] CreateFeedbackRequest request, IFormFile? image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (image != null)
            {
                var imageUrl = await _cloudinaryService.UploadImageAsync(image, "feedback");
                request.ImageUrl = imageUrl;
            }
            var result = await _feedbackUseCase.CreateFeedbackAsync(request);
            return Ok(result);
        }

        [HttpGet("shop/{shopId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetFeedbackResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFeedbacksByShopIdAsync(Guid shopId)
        {
            var result = await _feedbackUseCase.GetFeedbacksByShopIdAsync(shopId);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetFeedbackResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFeedbacksByCustomerIdAsync(Guid customerId)
        {
            var result = await _feedbackUseCase.GetFeedbacksByCustomerIdAsync(customerId);
            return Ok(result);
        }

        [HttpGet("order/{orderId}")]
        [ProducesResponseType(typeof(ApiResponse<GetFeedbackResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetFeedbackByOrderIdAsync(Guid orderId)
        {
            var result = await _feedbackUseCase.GetFeedbackByOrderIdAsync(orderId);
            return Ok(result);
        }

    }
}
