namespace OrderFood_BE.Application.Models.Response.Voucher
{
    public class GetVoucherResponse
    {
        public Guid Id { get; set; }
        public string VoucherCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool IsActive { get; set; }

        public Guid ShopId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

    }
}
