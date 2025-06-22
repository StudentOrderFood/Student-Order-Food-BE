using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrderFood_BE.Application.Repositories;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Infrastructure.Persistence.Repositories;
using OrderFood_BE.Infrastructure.Services;

namespace OrderFood_BE.Infrastructure.Extensions
{
    public static class ConfigureServiceExtension
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            _ = services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            _ = services.AddScoped<IUserRepository, UserRepository>();
            _ = services.AddScoped<IRoleRepository, RoleRepository>();
            _ = services.AddScoped<IShopRepository, ShopRepository>();

            _ = services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    } 
}
