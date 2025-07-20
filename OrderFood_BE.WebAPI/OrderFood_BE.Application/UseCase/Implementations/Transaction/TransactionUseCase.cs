using OrderFood_BE.Application.Models.Requests.Transaction;
using OrderFood_BE.Application.Models.Response.Order;
using OrderFood_BE.Application.Models.Response.Transaction;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.UseCase.Interfaces.Transaction;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                CreatedAt = DateTime.Now
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

    }
}
