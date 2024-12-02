using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ClientHistoryRepository(IConfiguration configuration)
    {
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();
        const string tableName = TableConstraint.ClientHistory;
        public async Task<bool> CreateHistory(ClientHistory clientHistory,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction =null)=>
            await warehouseConnection.CreateQueryStore(tableName,enabledSqlLogging)
            .InsertAsync(clientHistory,transaction);
    }
}
