using Microsoft.AspNetCore.Authentication;
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
            _ = services.AddScoped<IMenuItemRepository, MenuItemRepository>();
            _ = services.AddScoped<IVoucherRepository, VoucherRepository>();
            _ = services.AddScoped<IOrderRepository, OrderRepository>();
            _ = services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            _ = services.AddScoped<ICategoryRepository, CategoryRepository>();
            _ = services.AddScoped<ITransactionRepository, TransactionRepository>();

            _ = services.AddScoped<IJwtService, JwtService>();
            _ = services.AddScoped<ICloudinaryService, CloudinaryService>();

            
            return services;
        }
    } 
}
