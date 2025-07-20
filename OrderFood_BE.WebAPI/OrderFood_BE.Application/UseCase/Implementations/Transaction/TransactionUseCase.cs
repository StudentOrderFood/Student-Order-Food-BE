using OrderFood_BE.Application.Models.Requests.Transaction;
using OrderFood_BE.Application.Models.Response.Transaction;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Transaction;
using OrderFood_BE.Shared.Common;

namespace OrderFood_BE.Application.UseCase.Implementations.Transaction
{
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionUseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<ApiResponse<List<GetTransactionResponse>>> GetAllTransactionsByUserIdAsync(Guid userId)
        {
            var transactions = await _transactionRepository.GetAllTransactionsByUserIdAsync(userId);
            if (transactions == null || !transactions.Any())
                return ApiResponse<List<GetTransactionResponse>>.Fail("No transactions found for this user");

            var response = transactions.Select(t => new GetTransactionResponse
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Type = t.Type,
                OrderCode = t.OrderCode,
                Status = t.Status,
                PaymentTime = t.PaymentTime,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                OrderId = t.OrderId
            }).ToList();

            return ApiResponse<List<GetTransactionResponse>>.Ok(response, "Transactions retrieved successfully");
        }

        public async Task<ApiResponse<GetTransactionResponse>> RequestWithdrawAsync(WithdrawRequest request)
        {
            if (request == null || request.UserId == Guid.Empty || request.Amount <= 0)
                return ApiResponse<GetTransactionResponse>.Fail("Invalid withdraw request");

            var transaction = new Domain.Entities.HistoryTransaction
            {
                UserId = request.UserId,
                Amount = request.Amount,
                Description = request.Description ?? "Withdraw request",
                Type = "Withdraw",
                Status = "Pending",
                PaymentTime = DateTime.Now
            };

            await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            var response = new GetTransactionResponse
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                Type = transaction.Type,
                Status = transaction.Status,
                PaymentTime = transaction.PaymentTime,
                CreatedAt = transaction.CreatedAt,
                UserId = transaction.UserId
            };

            return ApiResponse<GetTransactionResponse>.Ok(response, "Withdraw request created successfully");
        }

        public async Task<ApiResponse<List<GetTransactionResponse>>> GetPendingWithdrawRequestsAsync()
        {
            var transactions = await _transactionRepository.GetPendingWithdrawRequestsAsync();
            if (!transactions.Any())
                return ApiResponse<List<GetTransactionResponse>>.Fail("No pending withdraw requests found");

            var response = transactions.Select(t => new GetTransactionResponse
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Type = t.Type,
                OrderCode = t.OrderCode,
                Status = t.Status,
                PaymentTime = t.PaymentTime,
                CreatedAt = t.CreatedAt,
                UserId = t.UserId,
                OrderId = t.OrderId
            }).ToList();

            return ApiResponse<List<GetTransactionResponse>>.Ok(response, "Pending withdraw requests retrieved successfully");
        }

        public async Task<ApiResponse<GetTransactionResponse>> ProcessWithdrawRequestAsync(WithdrawApprovalRequest request)
        {
            var transaction = await _transactionRepository.GetByIdAsync(request.TransactionId);
            if (transaction == null || transaction.Type != "Withdraw" || transaction.Status != "Pending")
            {
                return ApiResponse<GetTransactionResponse>.Fail("Invalid or already processed withdraw request");
            }

            transaction.Status = request.IsApproved ? "Success" : "Failed";
            transaction.Description += request.IsApproved ? "\nApproved by Admin" : "\nRejected by Admin";
            if (!string.IsNullOrWhiteSpace(request.AdminNote))
            {
                transaction.Description += $"\nNote: {request.AdminNote}";
            }

            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            var response = new GetTransactionResponse
            {
                Id = transaction.Id,
                Amount = transaction.Amount,
                Description = transaction.Description,
                Type = transaction.Type,
                Status = transaction.Status,
                PaymentTime = transaction.PaymentTime,
                CreatedAt = transaction.CreatedAt,
                UserId = transaction.UserId,
                OrderId = transaction.OrderId
            };

            return ApiResponse<GetTransactionResponse>.Ok(response, $"Withdraw request has been {(request.IsApproved ? "approved" : "rejected")}.");
        }

    }
}
