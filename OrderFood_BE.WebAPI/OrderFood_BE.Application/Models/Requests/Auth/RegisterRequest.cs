using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Auth
{
    public class RegisterRequest
    {
        public required string FullName { get; set; }
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public required string Phone { get; set; }
        public required string Address { get; set; }
        public string? Avatar { get; set; }
        public required string Email { get; set; }
        public required DateTime Dob { get; set; }
    }
}
