using Microsoft.Extensions.Configuration;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ClientAllowedObjectRepository(IConfiguration configuration)
    {
        const string tableName = TableConstraint.AllowedObject;
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();
        public async Task<List<AllowedObject>> ListAllowedObject(
             long clientId,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null
            ) =>
            await warehouseConnection.CreateQueryStore(tableName, enabledSqlLogging)
            .Where(nameof(AllowedObject.Client), clientId)
            .ToListAsync<AllowedObject>(transaction);
    }
}
