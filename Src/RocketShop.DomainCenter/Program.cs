
using LanguageExt;
using RocketShop.DomainCenter.Model;
using RocketShop.DomainCenter.Services;
using RocketShop.Framework.Extension;
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
            builder.InstallSerilog()
            .InstallServices(service =>
            {
                service.AddAuthorization()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddSingleton<Packageservices>();
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

            using var scope =app.Services.CreateScope();
            var _packageService = scope.ServiceProvider.GetService<Packageservices>();

            app.MapGet("/", (HttpContext httpContext) =>
            {
                return configuration.GetSection("Settings").Get<ConfigurationCenter>();
            })
            .WithName("GetConfiguration")
            .WithOpenApi();

            app.MapGet("/js/sweetalert", async () =>
            {
                var key = "https://cdn.jsdelivr.net/npm/sweetalert2@11";
                var queryResult = _packageService!.Find(key);
                if (queryResult.IsRight)
                {
                    string? value = queryResult.GetRight();
                    if (value.HasMessage()) return value;
                }
                
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(key);
                var newvalue = await result.Content.ReadAsStringAsync();
                _packageService.Add(key, newvalue);
                return newvalue;
            });
           

            app.Run();
        }
    }
}
