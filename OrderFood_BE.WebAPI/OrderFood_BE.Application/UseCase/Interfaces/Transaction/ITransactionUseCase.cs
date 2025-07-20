using OrderFood_BE.Application.Models.Requests.Transaction;
using OrderFood_BE.Application.Models.Response.Transaction;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Interfaces.Transaction
{
    public interface ITransactionUseCase
    {
        Task<ApiResponse<List<GetTransactionResponse>>> GetAllTransactionsByUserIdAsync(Guid userId);
        Task<ApiResponse<GetTransactionResponse>> RequestWithdrawAsync(WithdrawRequest request);
        Task<ApiResponse<List<GetTransactionResponse>>> GetPendingWithdrawRequestsAsync();
        Task<ApiResponse<GetTransactionResponse>> ProcessWithdrawRequestAsync(WithdrawApprovalRequest request);
    }
}
