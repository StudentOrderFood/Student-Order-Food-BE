using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class Feedback : BaseEntity<Guid>
    {
        public int Rating { get; set; }
        public string Content { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        public Guid CustomerId { get; set; }
        public User Customer { get; set; }
    }

}
