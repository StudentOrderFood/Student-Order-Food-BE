using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class Order : BaseEntity<Guid>
    {
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderTime { get; set; }
        public string FirebaseId { get; set; }

        public Guid CustomerId { get; set; }
        public User Customer { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public Guid? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
        public ICollection<HistoryTransaction> HistoryTransactions { get; set; }
    }

}
