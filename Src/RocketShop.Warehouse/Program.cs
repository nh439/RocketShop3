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

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.InstallConfiguration();
builder.InstallSerilog()  
    .InstallServices(repository =>
    {
        repository.AddScoped<ActivityLogRepository>()
        .AddScoped<ProvinceRepository>()
        .AddScoped<DistrictRepository>()
        .AddScoped<SubDistrictRepository>()
        .AddScoped<AddressViewRepository>();
    })
    .InstallServices(service =>
    {
        service.AddScoped<IActivityLogService, ActivityLogService>()
      .AddScoped<IExportExcelServices, ExportExcelServices>()
      .AddScoped<IImportExcelServices, ImportExcelServices>()
      .AddSingleton<IGetRoleAndPermissionService, GetRoleAndPermissionService>()
      .AddSingleton<IUrlIndeiceServices, UrlIndeiceServices>()
      .AddSingleton<IHttpContextAccessor,HttpContextAccessor>()
      .AddScoped<IAddressService,AddressServices>();
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

app.Run();
