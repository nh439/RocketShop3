using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ClientSecretRepository
    {
        const string tableName = TableConstraint.ClientSecret;

        public async Task<List<ClientSecret>> ListSecret(
            long clientId,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null
            ) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
            .Where(nameof(ClientSecret.Client), clientId)
            .ToListAsync<ClientSecret>();
    }
}
