using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OrderFood_BE.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken();
        Task<string> GenerateRefreshTokenAsync();
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    }
}
