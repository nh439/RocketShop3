using RocketShop.AuditService.Repository;
using RocketShop.AuditService.Services;
using RocketShop.Framework.Helper;
using RocketShop.Shared.SharedService.Scoped;
using RocketShop.Shared.SharedService.Singletion;
using RocketShop.Warehouse.Admin.Components;
using RocketShop.Database.Helper;
using RocketShop.Database.EntityFramework;

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
        baseService.InstallDatabase<IdentityContext, AuditLogContext, WarehouseContext>();
    })
    .InstallServices(repository =>
    {
        repository.AddScoped<ActivityLogRepository>();
    })
    .InstallServices(service =>
    {
        service.AddScoped<IActivityLogService, ActivityLogService>()
        .AddScoped<IExportExcelServices, ExportExcelServices>()
        .AddScoped<IImportExcelServices, ImportExcelServices>()
        .AddSingleton<IGetRoleAndPermissionService, GetRoleAndPermissionService>()
        .AddSingleton<IUrlIndeiceServices, UrlIndeiceServices>();
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

app.Run();
