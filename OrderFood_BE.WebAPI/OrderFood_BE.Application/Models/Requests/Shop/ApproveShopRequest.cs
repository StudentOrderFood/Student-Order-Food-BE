namespace OrderFood_BE.Application.Models.Requests.Shop
{
    public class ApproveShopRequest
    {
        public Guid ShopId { get; set; }
        public bool IsApproved { get; set; }
        public string? Note { get; set; }
    }
}
