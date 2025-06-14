using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Infrastructure.Options
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiryMinutes { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
    }
}
