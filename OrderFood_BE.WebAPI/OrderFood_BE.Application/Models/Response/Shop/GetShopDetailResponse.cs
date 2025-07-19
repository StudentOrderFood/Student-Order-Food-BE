using OrderFood_BE.Application.Models.Response.Category;
using OrderFood_BE.Application.Models.Response.MenuItem;

namespace OrderFood_BE.Application.Models.Response.Shop
{
    public class GetShopDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public double Rating { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        // List Category
        public List<GetCategoriesInShopMenu> Categories { get; set; }

        // List Items in MenuItems
        public List<GetMenuItemResponse> MenuItems { get; set; }

        public List<GetShopImageResponse> Images { get; set; }
    }
}
