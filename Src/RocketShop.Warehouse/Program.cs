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

var builder = WebApplication.CreateBuilder(args);

builder.InstallSerilog()  
    .InstallServices(repository =>
    {
        repository.AddScoped<ActivityLogRepository>()
        .AddScoped<ProvinceRepository>()
        .AddScoped<DistrictRepository>()
        .AddScoped<SubDistrictRepository>()
        .AddScoped<AddressViewRepository>()
        .AddScoped<ClientRepository>()
        .AddScoped<ClientSecretRepository>()
        .AddScoped<ClientAllowedObjectRepository>()
        .AddScoped<TokenRepository>()
        .AddScoped<ClientHistoryRepository>();
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
      .AddScoped<IAuthenicationService,AuthenicationService>();
    })
     .InstallServices(services =>
     {
         services.AddControllers();
         services.AddSwaggerGen()
         .AddEndpointsApiExplorer()
         .InstallDatabase<AuditLogContext, IdentityContext>()
         .AddGraphQLServer()
         .AddQueryType<GraphQuery>();
     });


var app = builder.Build();
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
