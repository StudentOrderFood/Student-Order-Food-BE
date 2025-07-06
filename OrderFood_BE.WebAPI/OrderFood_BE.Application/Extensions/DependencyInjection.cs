using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OrderFood_BE.Application.UseCase.Implementations.Auth;
using OrderFood_BE.Application.UseCase.Implementations.MenuItem;
using OrderFood_BE.Application.UseCase.Implementations.Shop;
using OrderFood_BE.Application.UseCase.Implementations.User;
using OrderFood_BE.Application.UseCase.Interfaces.Auth;
using OrderFood_BE.Application.UseCase.Interfaces.MenuItem;
using OrderFood_BE.Application.UseCase.Interfaces.Shop;
using OrderFood_BE.Application.UseCase.Interfaces.User;

namespace OrderFood_BE.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            _ = services.AddScoped<IAuthenticationUseCase, AuthenticationUseCase>();
            _ = services.AddScoped<IUserUseCase, UserUseCase>();
            _ = services.AddScoped<IShopUseCase, ShopUseCase>();
            _ = services.AddScoped<IMenuItemUseCase,MenuItemUseCase>();

            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            _ = services.AddUseCases();
            return services;
        }
    }

}
