using RocketShop.AuditService.Repository;
using RocketShop.AuditService.Services;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Helper;
using RocketShop.Framework.Helper;
using RocketShop.Shared.SharedService.Scoped;
using RocketShop.Shared.SharedService.Singletion;
using RocketShop.Warehouse.GraphQL;
using RocketShop.Warehouse.Repository;
using RocketShop.Warehouse.Services;
using RocketShop.Warehouse.Middleware;
using RocketShop.Shared.Extension;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.InstallConfiguration();
builder.InstallSerilog()  
    .InstallServices(repository =>
    {
        repository.AddScoped<ActivityLogRepository>()
        .AddScoped<ProvinceRepository>()
        .AddScoped<DistrictRepository>()
        .AddScoped<SubDistrictRepository>()
        .AddScoped<AddressViewRepository>()
        .AddSingleton<ClientRepository>()
        .AddSingleton<ClientSecretRepository>()
        .AddSingleton<ClientAllowedObjectRepository>()
        .AddSingleton<TokenRepository>()
        .AddSingleton<ClientHistoryRepository>();
    })
    .InstallServices(service =>
    {
        service.AddScoped<IActivityLogService, ActivityLogService>()
      .AddScoped<IExportExcelServices, ExportExcelServices>()
      .AddScoped<IImportExcelServices, ImportExcelServices>()
      .AddSingleton<IGetRoleAndPermissionService, GetRoleAndPermissionService>()
      .AddSingleton<IUrlIndeiceServices, UrlIndeiceServices>()
      .AddSingleton<IHttpContextAccessor,HttpContextAccessor>()
      .AddSingleton<IMemoryStorageServices, MemoryStorageServices>()
      .AddScoped<IAddressService,AddressServices>()
      .AddSingleton<IAuthenicationService,AuthenicationService>();
    })
     .InstallServices(services =>
     {
         services.AddControllers();
         services.AddSwaggerGen()
         .AddEndpointsApiExplorer()
         .InstallDatabase<AuditLogContext, IdentityContext>()
         .AddGraphQLServer()
         .AddQueryType<GraphQuery>();
         services.AddDistributedMemoryCache()
         .AddSession(options =>
         {
             options.IdleTimeout = TimeSpan.FromSeconds(10);
             options.Cookie.HttpOnly = true;
             options.Cookie.IsEssential = true;
         });
     });


var app = builder.Build();
var enabledSwagger = configuration.EnabledSwagger();
app.UseSession();
app.UseMachineAuthorization();
app.MapGraphQL(path:"/query");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
