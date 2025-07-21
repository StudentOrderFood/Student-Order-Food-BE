
using OrderFood.BackgroundServices;
using OrderFood_BE.Application.Extensions;
using OrderFood_BE.Infrastructure.Extensions;
using OrderFood_BE.WebAPI.Extensions;

namespace OrderFood_BE.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _ = builder.Services.AddApplication();
            _ = builder.Services.AddInfrastructure(builder.Configuration);
            _ = builder.Services.AddWebAPI(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpClient();
            builder.Services.AddHostedService<FirebaseOrderListenerService>();
            builder.Services.AddMemoryCache();

            // 
   //         builder.WebHost.UseUrls("https://0.0.0.0:7111");

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
               
            //}
            app.UseSwagger();
            app.UseSwaggerUI();
        //    app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.Run();
        }
    }
}
