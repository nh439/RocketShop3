using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientSecretRepository(WarehouseContext context,IConfiguration configuration)
    {
        readonly DbSet<ClientSecret> entity = context.ClientSecret;
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();

        public async Task<bool> CreateSecret(ClientSecret clientSecret) =>
            await entity.Add(clientSecret)
            .Context.SaveChangesAsync().GeAsync(0);

        public async Task<bool> DeleteSecret(string id)=>
            await entity.Where(x=>x.Id == id)
            .ExecuteDeleteAsync().GeAsync(0);

        public async Task<int> ClearSecret(long clientId,IDbConnection warehouseConnection,IDbTransaction? transaction = null)=>
            await warehouseConnection.CreateQueryStore(TableConstraint.ClientSecret,enabledSqlLogging)
            .Where(nameof(ClientSecret.Client),clientId)
            .DeleteAsync();

        public async Task<List<ClientSecret>> ListSecret(long clientId) =>
            await entity.Where(x => x.Client == clientId)
            .AsNoTracking()
            .ToListAsync();
    }
}
