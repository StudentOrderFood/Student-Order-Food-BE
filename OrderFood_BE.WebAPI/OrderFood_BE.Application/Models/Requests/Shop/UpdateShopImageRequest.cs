namespace OrderFood_BE.Application.Models.Requests.Shop
{
    public class UpdateShopImageRequest
    {
        public Guid ShopId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
