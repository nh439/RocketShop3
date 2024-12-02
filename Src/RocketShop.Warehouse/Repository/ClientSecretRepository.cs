using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ClientSecretRepository(IConfiguration configuration)
    {
        const string tableName = TableConstraint.ClientSecret;
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();

        public async Task<List<ClientSecret>> ListSecret(
            long clientId,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null
            ) =>
            await warehouseConnection.CreateQueryStore(tableName, enabledSqlLogging)
            .Where(nameof(ClientSecret.Client), clientId)
            .ToListAsync<ClientSecret>();
    }
}
