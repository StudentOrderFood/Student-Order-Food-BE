using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.User
{
    public class ProfileUpdateRequest
    {
        public required Guid UserId { get; set; }
        public required string FullName { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required DateTime Dob { get; set; }
        public required string Avatar { get; set; }
    }
}
