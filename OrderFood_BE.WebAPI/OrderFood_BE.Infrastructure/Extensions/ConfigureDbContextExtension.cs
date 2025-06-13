using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderFood_BE.Infrastructure.Options;
using OrderFood_BE.Infrastructure.Persistence.DBContext;

namespace OrderFood_BE.Infrastructure.Extensions
{
    public static class ConfigureDbContextExtension
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
            _ = services.AddDbContext<AppDbContext>(options =>
            {
                _ = options.UseSqlServer(databaseOptions?.ConnectionStrings);
                _ = options.EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
