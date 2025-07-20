using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFood_BE.Application.Models.Requests.Transaction;
using OrderFood_BE.Application.Models.Response.Transaction;
using OrderFood_BE.Application.UseCase.Interfaces.Transaction;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;

        public TransactionsController(ITransactionUseCase transactionUseCase)
        {
            _transactionUseCase = transactionUseCase;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetTransactionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTransactionsByUser(Guid userId)
        {
            var result = await _transactionUseCase.GetAllTransactionsByUserIdAsync(userId);
            return Ok(result);
        }

        [HttpPost("withdraw")]
        [ProducesResponseType(typeof(ApiResponse<GetTransactionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RequestWithdraw([FromBody] WithdrawRequest request)
        {
            var result = await _transactionUseCase.RequestWithdrawAsync(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("withdraw/pending")]
        [ProducesResponseType(typeof(ApiResponse<List<GetTransactionResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingWithdrawRequests()
        {
            var result = await _transactionUseCase.GetPendingWithdrawRequestsAsync();
            return Ok(result);
        }

        [HttpPost("withdraw/process")]
        [ProducesResponseType(typeof(ApiResponse<GetTransactionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ProcessWithdrawRequest([FromBody] WithdrawApprovalRequest request)
        {
            var result = await _transactionUseCase.ProcessWithdrawRequestAsync(request);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
