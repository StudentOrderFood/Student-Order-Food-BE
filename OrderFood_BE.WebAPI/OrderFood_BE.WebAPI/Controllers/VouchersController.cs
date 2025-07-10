using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Voucher;
using OrderFood_BE.Application.Models.Response.Voucher;
using OrderFood_BE.Application.UseCase.Interfaces.Voucher;
using OrderFood_BE.Shared.Common;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VouchersController : ControllerBase
    {
        private readonly IVoucherUseCase _voucherUseCase;

        public VouchersController(IVoucherUseCase voucherUseCase)
        {
            _voucherUseCase = voucherUseCase;
        }

        [HttpGet("shop/{shopId}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetVoucherResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVouchersByShopIdAsync(Guid shopId)
        {
            var vouchers = await _voucherUseCase.GetVouchersByShopIdAsync(shopId);
            return Ok(vouchers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetVoucherResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateVoucherAsync([FromBody] CreateVoucherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _voucherUseCase.CreateVoucherAsync(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetVoucherResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateVoucherAsync(Guid id, [FromBody] UpdateVoucherRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _voucherUseCase.UpdateVoucherAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteVoucherAsync(Guid id)
        {
            var result = await _voucherUseCase.DeleteVoucherAsync(id);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<GetVoucherResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVoucherByIdAsync(Guid id)
        {
            var voucher = await _voucherUseCase.GetVoucherByIdAsync(id);
            return Ok(voucher);
        }

        [HttpGet("available")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<GetVoucherResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAvailableVouchersAsync()
        {
            var vouchers = await _voucherUseCase.GetAllAvailableVouchersAsync();
            return Ok(vouchers);
        }

        [HttpGet("by-code/{code}")]
        [ProducesResponseType(typeof(ApiResponse<GetVoucherResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetVoucherByCodeAsync(string code)
        {
            var voucher = await _voucherUseCase.GetVoucherByCodeAsync(code);
            return Ok(voucher);
        }
    }
}
