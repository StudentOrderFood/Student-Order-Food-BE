using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Requests.Auth
{
    public class LoginRequest
    {
        public required string Identifier { get; set; }
        public required string Password { get; set; }
    }
}
