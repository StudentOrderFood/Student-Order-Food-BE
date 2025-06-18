using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class OrderItem : BaseEntity<Guid>
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Note { get; set; } = string.Empty;

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid ItemId { get; set; }
        public MenuItem Item { get; set; }
    }

}
