using Microsoft.AspNetCore.Identity;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Helper;
using RocketShop.Framework.Helper;
using RocketShop.HR.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RocketShop.HR.Repository;
using RocketShop.HR.Services;
using RocketShop.HR.ServicePermissions;
using RocketShop.Shared.SharedService.Singletion;
using RocketShop.Shared.SharedService.Scoped;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.InstallConfiguration();
builder.InstallSerilog()
    .InstallServices(install =>
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
        install.InstallIdentityContext()
        .AddRazorComponents()
    .AddInteractiveServerComponents();
        install.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        }).AddCookie(c => c.ExpireTimeSpan = TimeSpan.FromHours(10));
        install.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        install.AddAuthorization(a =>
        {
            a.AddPolicy(ServicePermission.AllHRServiceName, p =>
            {
                p.RequireClaim("Permission", ServicePermission.AllHRService);
            });
        });
    })
    .InstallServices(service =>
    {
        service.AddScoped<UserRepository>()
        .AddScoped<UserInformationRepository>();
    })
    .InstallServices(service =>
    {
        service.AddSingleton<IUrlIndeiceServices,UrlIndeiceServices>()
        .AddScoped<IUserServices,UserServices>()
        .AddScoped<IGetRoleAndPermissionService, GetRoleAndPermissionService>();
    });
// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
