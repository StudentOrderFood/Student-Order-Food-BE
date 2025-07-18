using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderFood_BE.Application.Models.Response.User;

namespace OrderFood_BE.Application.Models.Response.Shop
{
    public class GetPopularShopResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public TimeSpan OpenHours { get; set; }
        public TimeSpan EndHours { get; set; }
        public double Rating { get; set; }

        public List<Guid> CategoryIds { get; set; }
    }
}
