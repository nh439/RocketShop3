using RocketShop.AuditService.Repository;
using RocketShop.AuditService.Services;
using RocketShop.Framework.Helper;
using RocketShop.Shared.SharedService.Scoped;
using RocketShop.Shared.SharedService.Singletion;
using RocketShop.Warehouse.Admin.Components;
using RocketShop.Database.Helper;
using RocketShop.Database.EntityFramework;
using MudBlazor.Services;
using Radzen;
using RocketShop.SharedBlazor.SharedBlazorServices.Scope;
using Microsoft.AspNetCore.Identity;
using RocketShop.Database.Model.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using RocketShop.Warehouse.Admin.ServicePermission;
using RocketShop.Warehouse.Admin.Middleware;
using RocketShop.Warehouse.Admin.Repository;
using RocketShop.Warehouse.Admin.Services;
using RocketShop.SharedBlazor.SharedBlazorService.Scope;

var builder = WebApplication.CreateBuilder(args);
builder.InstallSerilog()
    .InstallConfiguration();
builder.InstallServices(install =>
{
    install.AddRazorComponents()
    .AddInteractiveServerComponents();
})
    .InstallServices(baseService =>
    {
        baseService.AddIdentity<User, IdentityRole>(option =>
        {
            option.SignIn.RequireConfirmedAccount = false;
            option.Password.RequireNonAlphanumeric = false;
        }).AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();
        baseService.AddIdentityCore<User>(s =>
        {
        })
.AddRoles<IdentityRole>()
.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>()
.AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();
        baseService.InstallDatabase<IdentityContext, AuditLogContext, WarehouseContext>()
        .AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        }).AddCookie(c => c.ExpireTimeSpan = TimeSpan.FromHours(10));
        baseService.AddAuthorization(a =>
        {
            a.AddPolicy(PolicyNames.AppCredentialManagerName, p =>
            {
                p.RequireClaim("Permission", PolicyPermissions.AppCredentialManagerPermissions);
            });
            a.AddPolicy(PolicyNames.DataMaintainerName, p =>
            {
                p.RequireClaim("Permission", PolicyPermissions.DataMaintainerPermissions);
            });
             a.AddPolicy(PolicyNames.AnyWHPolicyName, p =>
            {
                p.RequireClaim("Permission", PolicyPermissions.AnyWHPermissions);
            });


        })
        .AddMudServices()
        .AddRadzenComponents();
    })
    .InstallServices(repository =>
    {
        repository.AddScoped<ActivityLogRepository>()
        .AddScoped<TableInformationRepository>()
        .AddScoped<CollectionRepository>()
        .AddScoped<ClientRepository>()
        .AddScoped<ClientAllowedObjectRepository>()
        .AddScoped<ClientSecretRepository>();
    })
    .InstallServices(service =>
    {
        service.AddScoped<IActivityLogService, ActivityLogService>()
        .AddScoped<IExportExcelServices, ExportExcelServices>()
        .AddScoped<IImportExcelServices, ImportExcelServices>()
        .AddSingleton<IGetRoleAndPermissionService, GetRoleAndPermissionService>()
        .AddSingleton<IUrlIndeiceServices, UrlIndeiceServices>()
        .AddSingleton<IHttpContextAccessor,HttpContextAccessor>()
        .AddScoped<ISharedUserServices, SharedUserServices>()
        .AddScoped<ITableInformationService,TableInformationService>()
        .AddScoped<ICollectionServices,CollectionServices>()
        .AddScoped<IClientServices,ClientServices>()
        .AddScoped<IDialogServices,DialogServices>();
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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.UsePermissionMiddleware();
app.Run();
