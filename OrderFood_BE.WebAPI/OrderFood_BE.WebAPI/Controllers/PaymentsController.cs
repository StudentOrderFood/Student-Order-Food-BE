using System;
using System.Collections.Concurrent;
using System.Text.Json;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Order;
using OrderFood_BE.Application.Models.Requests.Payment;
using OrderFood_BE.Application.Models.Response.Payment;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Application.UseCase.Interfaces.Order;
using OrderFood_BE.Application.UseCase.Interfaces.User;
using OrderFood_BE.Infrastructure.Services;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPayOSService _payOSService;
        private readonly IFirebaseOrderSyncService _firebaseOrderSyncService;
        private readonly ITemporaryOrderCacheService _orderCache;
        private readonly IUserUseCase _userUseCase;


        public PaymentsController(IPayOSService payOSService, IFirebaseOrderSyncService firebaseOrderSyncService, ITemporaryOrderCacheService orderCache, IUserUseCase userUseCase)
        {
            _payOSService = payOSService;
            _firebaseOrderSyncService = firebaseOrderSyncService;
            _orderCache = orderCache;
            _userUseCase = userUseCase;
        }

        [HttpPost("create-payment")]
        [Authorize]
        public async Task<IActionResult> CreatePaymentQrCode([FromBody] BankingOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Lưu đơn hàng vào cache tạm
            //OrderCache[request.PayosOrderCode] = request;
            _orderCache.SaveOrder(request);

            var model = new PaymentRequestModel
            {
                OrderCode = request.PayosOrderCode,
                Amount = (int)request.TotalAmount,
                Description = $"Thanh toán đơn hàng đồ ăn",
                ReturnUrl = "https://studentorderfood.app/return",
                CancelUrl = "https://studentorderfood.app/cancel"
            };

            try
            {
                var checkoutUrl = await _payOSService.CreatePaymentLinkAsync(model);
                return Ok(new { checkoutUrl });
            }
            catch (HttpRequestException ex)
            {
                if (_orderCache.TryGetOrder(request.PayosOrderCode, out var order))
                {
                    _orderCache.RemoveOrder(request.PayosOrderCode);
                }
                return StatusCode(503, $"Failed to connect to PayOS: {ex.Message}");
            }
            catch (Exception ex)
            {
                if (_orderCache.TryGetOrder(request.PayosOrderCode, out var order))
                {
                    _orderCache.RemoveOrder(request.PayosOrderCode);
                }
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("payment-result")]
        [Authorize]
        public async Task<IActionResult> HandlePaymentResult([FromBody] PaymentResultRequest request)
        {
            if (!_orderCache.TryGetOrder(request.OrderCode, out var cachedOrder))
            {
                return NotFound("Order not found or expired from cache.");
            }

            // Hủy cache để tránh xử lý trùng
            _orderCache.RemoveOrder(request.OrderCode);

            if (request.Status == "PAID")
            {
                // Gọi UseCase để lưu vào DB hoặc xử lý logic
                await _firebaseOrderSyncService.PushOrderAsync(cachedOrder);
                var updateWallet = await _userUseCase.UpdateUserWallet(cachedOrder.ShopId, cachedOrder.TotalAmount);
                if (!updateWallet)
                {
                    return StatusCode(500, "Failed to update ShopOwner wallet balance.");
                }

                return Ok(new
                {
                    Message = "Payment successful. Order has been saved.",
                    OrderCode = request.OrderCode
                });
            }
            else
            {
                // Xử lý đơn bị cancel hoặc fail
                return Ok(new
                {
                    Message = $"Payment failed or was cancelled."
                });
            }
        }
    }
}
