using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.Auth
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserRole { get; set; }
        public string UserId { get; set; }
    }
}
