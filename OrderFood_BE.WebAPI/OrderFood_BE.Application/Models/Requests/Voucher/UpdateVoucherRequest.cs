namespace OrderFood_BE.Application.Models.Requests.Voucher
{
    public class UpdateVoucherRequest
    {
        public string VoucherCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal DiscountValue { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool IsActive { get; set; }
        public Guid ShopId { get; set; }

    }
}
