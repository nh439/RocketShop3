
using RocketShop.Framework.Helper;
using RocketShop.Shared.GlobalConfiguration;

namespace RocketShop.DomainCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.InstallConfiguration();
            builder.InstallServices(service =>
            {
                service.AddAuthorization()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/", (HttpContext httpContext) =>
            {
                return configuration.GetSection("Settings").Get<ConfigurationCenter>();
            })
            .WithName("GetConfiguration")
            .WithOpenApi();

            app.Run();
        }
    }
}
