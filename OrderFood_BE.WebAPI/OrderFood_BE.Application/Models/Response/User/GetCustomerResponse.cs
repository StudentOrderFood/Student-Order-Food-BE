using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.User
{
    public class GetCustomerResponse
    {
        public required Guid UserId { get; set; }
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
    }
}
