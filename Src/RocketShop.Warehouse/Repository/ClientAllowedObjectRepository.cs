using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ClientAllowedObjectRepository
    {
        const string tableName = TableConstraint.AllowedObject;

        public async Task<List<AllowedObject>> ListAllowedObject(
             long clientId,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null
            ) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
            .Where(nameof(AllowedObject.Client), clientId)
            .ToListAsync<AllowedObject>(transaction);
    }
}
