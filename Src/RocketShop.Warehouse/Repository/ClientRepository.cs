using LanguageExt;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ClientRepository
    {
        const string tableName = TableConstraint.Client;
        public async Task<Option<Client>> GetByClientName(string clientId,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
            .Where(nameof(Client.ClientId), clientId)
            .FetchOneAsync<Client>(transaction);

        public async Task<Option<string>> GetClientIdById(long id,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName)
            .Where(nameof(Client.Id),id)
            .Select(nameof(Client.ClientId))
            .FetchOneAsync<string>(transaction);
    }
}
