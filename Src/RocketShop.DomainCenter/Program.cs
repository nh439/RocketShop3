
using LanguageExt;
using Microsoft.AspNetCore.Http;
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
                .AddScoped<IEmailServices,EmailServices>()
                .AddScoped<IFilePathServices,FilePathServices>();
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
            var _fileService = scope.ServiceProvider.GetService<IFilePathServices>();

            app.MapGet("/Urls", (HttpContext httpContext) =>
            {
                return configuration.GetSection("Settings").Get<ConfigurationCenter>();
            })
            .WithName("GetConfiguration")
            .WithOpenApi();

            app.MapGet("/sweetalert", async () =>
            {
                string filename = "sweetalert";
                var hasfile = _fileService!.CheckFileExists(filename);
                if (hasfile)
                    return _fileService.GetContent(filename);
                var url = "https://cdn.jsdelivr.net/npm/sweetalert2@11";          
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(url);
                var newvalue = await result.Content.ReadAsStringAsync();
                _fileService.Create(filename, newvalue);
                return newvalue;
            });

            app.MapGet("/tailwind",async () =>{
                string filename = "tailwind";
                var hasfile = _fileService!.CheckFileExists(filename);
                if (hasfile)
                    return _fileService.GetContent(filename);
                var url = "https://cdn.tailwindcss.com";
                var value = _packageService!.Find(url);
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(url);
                var newvalue = await result.Content.ReadAsStringAsync();
                _fileService.Create(filename, newvalue);
                return newvalue;
            });

            app.MapGet("/semantic",async () =>{
                string filename = "semantic";
                var hasfile = _fileService!.CheckFileExists(filename);
                if (hasfile)
                    return _fileService.GetContent(filename);
                var url = "https://cdn.jsdelivr.net/npm/semantic-ui@2.5.0/dist/semantic.min.css";
                var value = _packageService!.Find(url);
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(url);
                var newvalue = await result.Content.ReadAsStringAsync();
                _fileService.Create(filename, newvalue);
                return newvalue;
            });

            app.MapGet("/bootstrap-icons", async () =>{
                string filename = "bootstrap-icons";
                var hasfile = _fileService!.CheckFileExists(filename);
                if (hasfile)
                    return _fileService.GetContent(filename);
                var url = "https://cdn.jsdelivr.net/npm/bootstrap-icons@1.3.0/font/bootstrap-icons.css";
                var value = _packageService!.Find(url);
                using var httpclient = new HttpClient();
                var result = await httpclient.GetAsync(url);
                var newvalue = await result.Content.ReadAsStringAsync();
                _fileService.Create(filename, newvalue);
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
            app.MapGet("bind", (HttpContext httpContext) => configuration.GetSection("BindMount").Value);
            app.MapGet("/services", (HttpContext httpContext) => configuration.GetRequiredSection("RegisterdServices").Get<string[]>());
            app.Run();
        }
    }
}
