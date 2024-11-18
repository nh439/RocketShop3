using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;
using System.Linq.Dynamic.Core;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientAllowedObjectRepository(WarehouseContext context)
    {
        readonly string tableName = TableConstraint.AllowedObject;
        readonly DbSet<AllowedObject> entity = context.AllowedObject;
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
                .BulkInsertAsync(allowedObjects,transaction);
            return true;
        }

        public async Task<List<AllowedObject>> GetAllowedObject(long clientId) =>
            await entity.Where(x=>x.Client == clientId).ToListAsync();

    }
}
