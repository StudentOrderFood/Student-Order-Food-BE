namespace OrderFood_BE.Application.Models.Requests.Shop
{
    public class UpdateShopRequest
    {
        public Guid ShopId { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? Address { get; set; }
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public string? BusinessLicenseImageUrl { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid OwnerId { get; set; }
    }
}
