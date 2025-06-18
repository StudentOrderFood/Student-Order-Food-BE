using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class ShopImage : BaseEntity<Guid>
    {
        public string ImageUrl { get; set; } = string.Empty;

        public Guid ShopId { get; set; }
        public Shop Shop { get; set; }
    }

}
