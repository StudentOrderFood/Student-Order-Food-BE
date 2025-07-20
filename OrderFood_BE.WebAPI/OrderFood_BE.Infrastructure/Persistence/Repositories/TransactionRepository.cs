using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository(AppDbContext context) : GenericRepository<HistoryTransaction, Guid>(context), ITransactionRepository
    {
        public async Task<IEnumerable<HistoryTransaction>> GetAllTransactionsByUserIdAsync(Guid userId)
        {
            return await _context.HistoryTransactions
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
