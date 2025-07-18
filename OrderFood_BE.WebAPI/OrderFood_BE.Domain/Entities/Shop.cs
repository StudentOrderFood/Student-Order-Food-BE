using OrderFood_BE.Domain.Base;

namespace OrderFood_BE.Domain.Entities
{
    public class Shop : BaseEntity<Guid>
    {
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; }
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public double Rating { get; set; }

        public string? BusinessLicenseImageUrl { get; set; }
        public string? Note { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<MenuItem> MenuItems { get; set; }
        public ICollection<ShopImage> ShopImages { get; set; }
        public ICollection<Voucher> Vouchers { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

}
