
using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using RocketShop.DomainCenter.Model;
using RocketShop.DomainCenter.Services;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Helper;
using RocketShop.Shared.GlobalConfiguration;
using RocketShop.Shared.Model;

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
                .AddSingleton<Packageservices>()
                .AddScoped<IEmailServices,EmailServices>();
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
            var _emailService = scope.ServiceProvider.GetService<IEmailServices>();

            app.MapGet("/", (HttpContext httpContext) =>
            {
                return configuration.GetSection("Settings").Get<ConfigurationCenter>();
            })
            .WithName("GetConfiguration")
            .WithOpenApi();

            app.MapGet("/js/sweetalert", async () =>
            {
                var key = "https://cdn.jsdelivr.net/npm/sweetalert2@11";          
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(key);
                var newvalue = await result.Content.ReadAsStringAsync();
                return newvalue;
            });

            app.MapGet("/css/tailwind",async () =>{
                var key = "https://cdn.tailwindcss.com";
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(key);
                var newvalue = await result.Content.ReadAsStringAsync();
                return newvalue;
            });

            app.MapPost("/mail", async ([FromBody] MailRequest req) =>
            {
                var sendResult = await _emailService.SendAsync(req);
                if (sendResult.IsLeft)
                {
                    Results.Problem(sendResult.GetLeft()!.Message, statusCode: 500);
                    return sendResult.GetLeft()!.Message;
                }
                return sendResult.GetRight();
            });
            app.Run();
        }
    }
}
