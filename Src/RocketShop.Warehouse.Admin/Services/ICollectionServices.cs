using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Framework.Services;
using RocketShop.Warehouse.Admin.Model;
using RocketShop.Warehouse.Admin.Repository;

namespace RocketShop.Warehouse.Admin.Services
{
    public interface ICollectionServices
    {
        Task<Either<Exception, FlexibleDataReport>> CallData(
            string collectionName,
            string? orderBy = null,
            bool asc = true,
            int? page = null,
            int per = 50
            );

        Task<Either<Exception, FlexibleDataReport>> CallDataWithCondition(
            string collectionName,
            string where,
            string? orderBy = null,
            bool asc = true,
            int? page = null,
            int per = 50
            );
    }
    public class CollectionServices
        (
        IConfiguration configuration,
        ILogger<CollectionServices> logger,
        CollectionRepository repository
        ) : BaseServices<CollectionServices>
        (
            "Collection Service", 
            logger, 
            new NpgsqlConnection(configuration.GetWarehouseConnectionString())
        ),
        ICollectionServices
    {
        public async Task<Either<Exception, FlexibleDataReport>> CallData(
            string collectionName, 
            string? orderBy = null, 
            bool asc = true, 
            int? page = null, 
            int per = 50
            ) =>
            await InvokeDapperServiceAsync(async warehouseConnection => await repository.CallData
            (
                collectionName, 
                orderBy, 
                asc, 
                page, 
                per, 
                (warehouseConnection as NpgsqlConnection)!)
            );
        public async Task<Either<Exception, FlexibleDataReport>> CallDataWithCondition(
            string collectionName, 
            string where,
            string? orderBy = null, 
            bool asc = true, 
            int? page = null, 
            int per = 50
            ) =>
            await InvokeDapperServiceAsync(async warehouseConnection => await repository.CallDataWithCondition
            (
                collectionName, 
                where,
                orderBy, 
                asc, 
                page, 
                per, 
                (warehouseConnection as NpgsqlConnection)!)
            );

    }


}
