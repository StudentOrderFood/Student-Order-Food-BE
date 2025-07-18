namespace OrderFood_BE.Application.Models.Requests.Shop
{
    public class CreateShopRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public string? BusinessLicenseImageUrl { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Guid OwnerId { get; set; }
    }
}
