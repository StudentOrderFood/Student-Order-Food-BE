using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface ITransactionRepository : IGenericRepository<HistoryTransaction, Guid>
    {
        Task<IEnumerable<HistoryTransaction>> GetAllTransactionsByUserIdAsync(Guid userId);
        Task<IEnumerable<HistoryTransaction>> GetPendingWithdrawRequestsAsync();
    }
}
