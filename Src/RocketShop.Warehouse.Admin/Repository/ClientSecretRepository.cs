using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Framework.Extension;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientSecretRepository(WarehouseContext context)
    {
        readonly DbSet<ClientSecret> entity = context.ClientSecret;

        public async Task<bool> CreateSecret(ClientSecret clientSecret) =>
            await entity.Add(clientSecret)
            .Context.SaveChangesAsync().GeAsync(0);

        public async Task<bool> DeleteSecret(string id)=>
            await entity.Where(x=>x.Id == id)
            .ExecuteDeleteAsync().GeAsync(0);

    }
}
