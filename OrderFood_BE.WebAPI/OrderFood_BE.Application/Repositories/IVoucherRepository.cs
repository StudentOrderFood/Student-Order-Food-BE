using OrderFood_BE.Domain.Entities;

namespace OrderFood_BE.Application.Repositories
{
    public interface IVoucherRepository : IGenericRepository<Voucher, Guid>
    {
        Task<Voucher> GetByVoucherCodeAsync(string code);
        Task<IEnumerable<Voucher>> GetVouchersByShopIdAsync(Guid shopId);
        Task<IEnumerable<Voucher>> GetAvailableVouchersAsync();
    }
}
