using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class MenuItem : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }

}
