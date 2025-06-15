namespace OrderFood_BE.WebAPI.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPI(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddCustomSwagger();

            return services;
        }
    }
}
