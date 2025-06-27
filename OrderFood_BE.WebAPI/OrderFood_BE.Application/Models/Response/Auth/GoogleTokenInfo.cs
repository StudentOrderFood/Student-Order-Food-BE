using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Models.Response.Auth
{
    public class GoogleTokenInfo
    {
        public string Email { get; set; }
        [JsonPropertyName("email_verified")]
        public string EmailVerified { get; set; }
        public string Aud { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
    }
}
