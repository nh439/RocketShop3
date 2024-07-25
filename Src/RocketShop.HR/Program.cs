using Microsoft.AspNetCore.Identity;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Helper;
using RocketShop.Framework.Helper;
using RocketShop.HR.Components;

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
        install.InstallIdentityContext();
    });
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
