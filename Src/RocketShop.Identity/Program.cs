using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Helper;
using RocketShop.Identity.Configuration;

namespace RocketShop.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.InstallConfiguration();
            var oauthConfiguration = configuration.GetSection("OauthConfiguration").Get<OauthConfiguration>();
            // Add services to the container.
            builder.InstallServices(install =>
            {
                install.AddIdentity<User, IdentityRole>(option =>
                {
                    option.SignIn.RequireConfirmedAccount = false;
                    option.Password.RequireNonAlphanumeric = false;
                }).AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();
                install.AddIdentityCore<User>(s =>
                {
                })
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();
                install.AddControllersWithViews();
                install.AddDbContext<IdentityContext>();
                install.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                }).AddCookie();
                install.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
