using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OrderFood_BE.Infrastructure.Options;


namespace OrderFood_BE.Infrastructure.Extensions
{
    public static class ConfigureJwtExtension
    {
        public static IServiceCollection AddCustomJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();

            _ = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var key = UTF8Encoding.UTF8.GetBytes(jwtSettings?.SecretKey ?? string.Empty);
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = jwtSettings?.Audience,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            _ = services.AddAuthorization();

            return services;
        }
    }
}
