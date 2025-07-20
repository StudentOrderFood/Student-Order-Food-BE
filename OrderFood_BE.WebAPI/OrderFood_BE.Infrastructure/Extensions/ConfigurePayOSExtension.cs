using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderFood_BE.Application.Services;
using OrderFood_BE.Infrastructure.Options;
using OrderFood_BE.Infrastructure.Services;

namespace OrderFood_BE.Infrastructure.Extensions
{
    public static class ConfigurePayOSExtension
    {
        public static IServiceCollection AddPayOS(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("PayOS");
            var settings = section.Get<PayOSSettings>() ?? new PayOSSettings();

            services.AddSingleton(settings);
            services.AddHttpClient<IPayOSService, PayOSService>();

            return services;
        }
    }
}
