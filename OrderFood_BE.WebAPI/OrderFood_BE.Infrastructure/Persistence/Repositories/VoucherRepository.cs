using Microsoft.EntityFrameworkCore;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Persistence.Repositories
{
    public class VoucherRepository(AppDbContext context) : GenericRepository<Voucher, Guid>(context), IVoucherRepository
    {
        public async Task<Voucher> GetByVoucherCodeAsync(string code)
        {
            return await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherCode == code && v.IsActive);
        }

        public async Task<IEnumerable<Voucher>> GetVouchersByShopIdAsync(Guid shopId)
        {
            return await _context.Vouchers
                .Where(v => v.ShopId == shopId && v.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Voucher>> GetAvailableVouchersAsync()
        {
            return await _context.Vouchers
                .Where(v => v.IsActive && v.Count > 0)
                .ToListAsync();
        }
    }
}
