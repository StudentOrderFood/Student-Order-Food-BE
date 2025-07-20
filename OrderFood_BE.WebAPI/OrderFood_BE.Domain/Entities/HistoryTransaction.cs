using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class HistoryTransaction : BaseEntity<Guid>
    {
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Ví dụ: "Payment", "TopUp", "Withdraw"
        public string OrderCode { get; set; } = string.Empty; // "Pending", "Success", "Failed"
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentTime { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? OrderId { get; set; }
        public Order? Order { get; set; }
    }

}
