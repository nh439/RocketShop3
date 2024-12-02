using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientHistoryRepository(WarehouseContext context,IConfiguration configuration)
    {
        readonly DbSet<ClientHistory> entity = context.ClientHistory;
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();

        public async Task<List<ClientHistory>> CallHistory(long clientId,int? page,int per)=>
            await entity.Where(x=>x.ClientId==clientId)
            .OrderByDescending(x=>x.Date)
            .UsePaging(page,per)
            .ToListAsync();

        public async Task<int> ClearHistory(long clientId, IDbConnection warehouseConnection, IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(TableConstraint.ClientHistory,enabledSqlLogging)
            .Where(nameof(ClientHistory.ClientId), clientId)
            .DeleteAsync();

        public async Task<int> GetCount(long clientId)=>
            await entity.CountAsync(x=>x.ClientId == clientId);

        public async Task<int> GetLastpage(long clientId,int per)=>
            await entity.GetLastpageAsync(x=>x.ClientId==clientId,per);
    }
}
