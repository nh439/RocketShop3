using RocketShop.Framework.Helper;
using RocketShop.Retail.Components;
using RocketShop.Shared.SharedService.Singletion;
using RocketShop.Warehouse.Admin.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.InstallConfiguration();
builder.InstallSerilog()
   .InstallServices(services =>
   services.AddRazorComponents()
    .AddInteractiveServerComponents()
   )
   .InstallServices(repositories =>
   {

   })
   .InstallServices(services=> {
       services
       .AddSingleton<IUrlIndeiceServices,UrlIndeiceServices>()
       .AddSingleton<IWarehouseAuthenicationServices, WarehouseAuthenicationServices>()
       .AddSingleton<IGetRoleAndPermissionService,GetRoleAndPermissionService>();
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
