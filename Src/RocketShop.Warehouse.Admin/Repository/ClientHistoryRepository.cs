using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientHistoryRepository(WarehouseContext context)
    {
        readonly DbSet<ClientHistory> entity = context.ClientHistory;

        public async Task<List<ClientHistory>> CallHistory(long clientId,int? page,int per)=>
            await entity.Where(x=>x.ClientId==clientId)
            .OrderByDescending(x=>x.Date)
            .UsePaging(page,per)
            .ToListAsync();

        public async Task<int> ClearHistory(long clientId, IDbConnection warehouseConnection, IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(TableConstraint.ClientHistory)
            .Where(nameof(ClientHistory.ClientId), clientId)
            .DeleteAsync();
    }
}
