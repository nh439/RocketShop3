using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientAllowedObjectRepository
    {
        readonly string tableName = TableConstraint.AllowedObject;
        public async Task<bool> SetAllowedObject(
            long clientId,
            List<AllowedObject> allowedObjects,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null)
        {
            await allowedObjects.HasDataAndForEachAsync(a => a.Client = clientId);
            await warehouseConnection.CreateQueryStore(tableName,true)
                .Where(nameof(AllowedObject.Client),clientId)
                .DeleteAsync(transaction);
            await warehouseConnection.CreateQueryStore(tableName,true)
                .InsertAsync(allowedObjects,transaction);
            return true;
        }
    }
}
