using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.Models.Response.Order;
using OrderFood_BE.Application.UseCase.Interfaces.Order;
using OrderFood_BE.Shared.Common;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderUseCase _orderUseCase;

        public OrdersController(IOrderUseCase orderUseCase)
        {
            _orderUseCase = orderUseCase;
        }

        [HttpGet("shop/{shopId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetOrderResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByShopIdAsync(Guid shopId)
        {
            var orders = await _orderUseCase.GetOrdersByShopIdAsync(shopId);
            return Ok(orders);
        }

        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetOrderResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderUseCase.GetOrdersByCustomerIdAsync(customerId);
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetOrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderUseCase.GetOrderByIdAsync(id);
            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetOrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _orderUseCase.CreateOrderAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}/cancel")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CancelOrderAsync(Guid id)
        {
            var result = await _orderUseCase.CancelOrderAsync(id);
            return Ok(result);
        }
        
    }
}
