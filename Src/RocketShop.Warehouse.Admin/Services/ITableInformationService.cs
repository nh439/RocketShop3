using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Framework.Services;
using RocketShop.Warehouse.Admin.Model;
using RocketShop.Warehouse.Admin.Repository;
using System.Data;

namespace RocketShop.Warehouse.Admin.Services
{
    public interface ITableInformationService
    {
        Task<Either<Exception, IEnumerable<string>>> ListTableNames();
        Task<Either<Exception, IEnumerable<string>>> ListViewNames();
        Task<Either<Exception, List<TableDescription>>> ListTableInformation(string? search = null, int? page = null, int per = 5);
        Task<Either<Exception, int>> CountCollections(string? search = null);
        Task<Either<Exception, int>> GetCollectionsLastPage(string? search = null, int per = 5);
    }
    public class TableInformationService(
        ILogger<TableInformationService> logger,
        IConfiguration configuration,
        TableInformationRepository tableInformationRepository
        ) : BaseServices<TableInformationService>(
            "Table Information Service",
            logger,
            new NpgsqlConnection(configuration.GetWarehouseConnectionString())),ITableInformationService
            {
        public async Task<Either<Exception, IEnumerable<string>>> ListTableNames() =>
            await InvokeDapperServiceAsync(async WarehouseConnection => await tableInformationRepository.ListTableNames(WarehouseConnection));

        public async Task<Either<Exception, IEnumerable<string>>> ListViewNames() =>
            await InvokeDapperServiceAsync(async WarehouseConnection => await tableInformationRepository.ListViewNames(WarehouseConnection));

        public async Task<Either<Exception, List<TableDescription>>> ListTableInformation(string? search = null, int? page = null, int per = 5) =>
            await InvokeDapperServiceAsync(async warehouseConnection =>
            {
                if(warehouseConnection.State != System.Data.ConnectionState.Open)
                    warehouseConnection.Open();
                return await tableInformationRepository.GetTableInformations(search, page, per, warehouseConnection);
            });

        public async Task<Either<Exception, int>> CountCollections(string? search = null) =>
        await InvokeDapperServiceAsync(async warehouseConnection => await tableInformationRepository.CountCollections(search,warehouseConnection));

        public async Task<Either<Exception, int>> GetCollectionsLastPage(string? search = null, int per = 5) =>
             await InvokeDapperServiceAsync(async warehouseConnection =>
             {
                 var count = await tableInformationRepository.CountCollections(search, warehouseConnection);
                 return (int)Math.Ceiling((decimal)count / (decimal)per);
             });
            }
    
}
