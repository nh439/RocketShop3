using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using MudBlazor.Services;
using Radzen;
using RocketShop.AuditService.Repository;
using RocketShop.AuditService.Services;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Helper;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Helper;
using RocketShop.Framework.Services;
using RocketShop.Retail.Components;
using RocketShop.Shared.SharedService.Singletion;
using RocketShop.SharedBlazor.SharedBlazorService.Scope;
using RocketShop.SharedBlazor.SharedBlazorServices.Scope;
using RocketShop.Warehouse.Admin.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.InstallConfiguration();
builder.InstallSerilog()
   .InstallServices(services =>
   services.AddRazorComponents()
    .AddInteractiveServerComponents()
   )
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
       install.InstallDatabase<AuditLogContext, IdentityContext>()
       .AddHttpContextAccessor()
       .AddAuthentication(options =>
       {
           options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
           options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
       }).AddCookie(c => c.ExpireTimeSpan = TimeSpan.FromHours(10));
       install.AddAuthorization(a =>
       {

       })
        .AddMudServices()
        .AddRadzenComponents();
   })
   .InstallServices(repositories =>
   {
       repositories.AddScoped<ActivityLogRepository>();
   })
   .InstallServices(services =>
   {
       services
       .AddSingleton<IUrlIndeiceServices, UrlIndeiceServices>()
       .AddSingleton<IWarehouseAuthenicationServices, WarehouseAuthenicationServices>()
       .AddSingleton<IGetRoleAndPermissionService, GetRoleAndPermissionService>()
       .AddSingleton<IWarehouseQueryServices, WarehouseQueryServices>()
       .AddScoped<IActivityLogService, ActivityLogService>()
       .AddScoped<ISharedUserServices, SharedUserServices>()
       .AddScoped<IDialogServices, DialogServices>();
   });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UsePermissionMiddleware();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
