namespace OrderFood_BE.Application.Models.Requests.Shop
{
    public class UpdateShopRequest
    {
        public Guid ShopId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public Guid OwnerId { get; set; }
    }
}
