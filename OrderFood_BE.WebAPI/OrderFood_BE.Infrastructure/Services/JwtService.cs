using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Domain.Entities;
using OrderFood_BE.Infrastructure.Options;

namespace OrderFood_BE.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IGenericRepository<RefreshToken, Guid> _repository;
        public JwtService(IOptions<JwtSettings> jwtSettings, IGenericRepository<RefreshToken, Guid> repository)
        {
            _jwtSettings = jwtSettings.Value;
            _repository = repository;
        }
        public string GenerateAccessToken()
        {
            var claims = new Claim[] { };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _jwtSettings.Issuer, audience: _jwtSettings.Audience, claims: claims, expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateRefreshTokenAsync(Guid userId)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiry = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryDays);
            var tokenEntity = new RefreshToken
            {
                Token = refreshToken,
                ExpiryDate = expiry,
                UserId = userId
            };

            _ = await _repository.AddAsync(tokenEntity);
            await _repository.SaveChangesAsync();

            return refreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
        {
            var tokenEntity = (await _repository.FindAsync(x => x.Token == refreshToken, asNoTracking: true)).FirstOrDefault();

            return tokenEntity != null &&
                   tokenEntity.UserId == userId &&
                   tokenEntity.ExpiryDate > DateTime.UtcNow;
        }
    }
}
