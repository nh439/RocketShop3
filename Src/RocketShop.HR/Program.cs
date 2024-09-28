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
using RocketShop.HR.Middleware;
using RocketShop.SharedBlazor.SharedBlazorService.Scope;
using RocketShop.Shared.SharedService.Scoped;
using RocketShop.SharedBlazor.SharedBlazorServices.Scope;
using Radzen;
using RocketShop.AuditService.Repository;
using RocketShop.AuditService.Services;

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
        install.InstallDatabase<IdentityContext, AuditLogContext>()
        .AddRazorComponents()
    .AddInteractiveServerComponents();
        install.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        }).AddCookie(c => c.ExpireTimeSpan = TimeSpan.FromHours(10));
        install.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        install.AddAuthorization(a =>
        {
            a.AddPolicy(ServicePermission.AllHRServiceName, p =>
            {
                p.RequireClaim("Permission", ServicePermission.AllHRService);
            });
            a.AddPolicy(ServicePermission.HRFinancialName, p =>
            {
                p.RequireClaim("Permission", ServicePermission.HRFinancial);
            });
            a.AddPolicy(ServicePermission.HRAuditName, p =>
            {
                p.RequireClaim("Permission", ServicePermission.HRAuditLog);
            });
            a.AddPolicy(ServicePermission.HREmployeeName, p =>
                   {
                       p.RequireClaim("Permission", ServicePermission.HREmployee);
                   });
             a.AddPolicy(ServicePermission.AppAdminName, p =>
                   {
                       p.RequireClaim("Permission", ServicePermission.ApplicationAdmin);
                   });

        })
        .AddRadzenComponents();
    })
    .InstallServices(service =>
    {
        service.AddScoped<UserRepository>()
        .AddScoped<UserInformationRepository>()
        .AddScoped<RoleRepository>()
        .AddScoped<UserRoleRepository>()
        .AddScoped<ChangePasswordHistoryRepository>()
        .AddScoped<UserFinacialRepository>()
        .AddScoped<ProvidentRepository>()
        .AddScoped<UserAdditionalExpenseRepository>()
        .AddScoped<UserPayrollRepository>()
        .AddScoped<AdditionalPayrollRepository>()
        .AddScoped<ActivityLogRepository>();
    })
    .InstallServices(service =>
    {
        service.AddSingleton<IUrlIndeiceServices, UrlIndeiceServices>()
        .AddScoped<IUserServices, UserServices>()
        .AddSingleton<IGetRoleAndPermissionService, GetRoleAndPermissionService>()
        .AddScoped<IRoleServices, RoleServices>()
        .AddScoped<IDialogServices, DialogServices>()
        .AddScoped<IImportExcelServices, ImportExcelServices>()
        .AddScoped<IExportExcelServices, ExportExcelServices>()
        .AddScoped<IDownloadServices, DownloadServices>()
        .AddScoped<IFinacialServices, FinacialServices>()
        .AddScoped<IPayrollServices, PayrollServices>()
        .AddScoped<IActivityLogService, ActivityLogService>();
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
app.UsePermissionsMiddleware();
app.Run();
