using OrderFood_BE.Application.Models.Response.User;

namespace OrderFood_BE.Application.Models.Response.Shop
{
    public class GetShopResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public double Rating { get; set; }

        public Guid OwnerId { get; set; }
        public GetUserResponse Owner { get; set; }
        public List<GetShopImageResponse> Images { get; set; }
    }
}
