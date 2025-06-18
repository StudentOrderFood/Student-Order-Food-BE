using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class Voucher : BaseEntity<Guid>
    {
        public string VoucherCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool IsActive { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

}
