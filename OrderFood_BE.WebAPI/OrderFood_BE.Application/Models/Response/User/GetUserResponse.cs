using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.User
{
    public class GetUserResponse
    {
        public required Guid UserId { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public required DateTime Dob { get; set; }
        public required string Avatar { get; set; }
        public required decimal WalletBalance { get; set; }
        public required Guid RoleId { get; set; }
        public required string RoleName { get; set; }
    }
}
