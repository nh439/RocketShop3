using DocumentFormat.OpenXml.Presentation;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;
using System.Linq.Dynamic.Core;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class ClientRepository(WarehouseContext context)
    {
        readonly DbSet<Client> entity = context.Client;
        public async Task<List<Client>> ListClient(string? search, int? page, int per)
        {
            var result = entity.AsQueryable();
            if (search.HasMessage())
                result = result.Where(x =>
                x.ClientId.ToLower().Contains(search!.ToLower()) ||
                x.Application!.ToLower()!.Contains(search!.ToLower()) ||
                x.ClientName.ToLower().Contains(search!.ToLower())
                );
            result = result.OrderByDescending(x => x.Id);
            if (page.HasValue)
                result = result.UsePaging(page, per);
            return await result.ToListAsync();
        }

        public async Task<Client?> GetClient(long clientId) =>
            await entity.FirstOrDefaultAsync(x => x.Id == clientId);

        public async Task<long> CountClient(string? search) =>
            search.HasMessage() ? 
            await entity.CountAsync(x =>
                x.ClientId.ToLower().Contains(search!.ToLower()) ||
                x.Application!.ToLower()!.Contains(search!.ToLower())  ||
                x.ClientName.ToLower().Contains(search!.ToLower())) :
            await entity.CountAsync();

        public async Task<int> GetLastpage(string? search,int per)=>
             search.HasMessage() ?
            await entity.Where(x =>
                x.ClientId.ToLower().Contains(search!.ToLower()) ||
                x.Application!.ToLower()!.Contains(search!.ToLower()) ||
                x.ClientName.ToLower().Contains(search!.ToLower())).GetLastpageAsync(per) :
            await entity.GetLastpageAsync(per);

        public async Task<Client> Create(Client client)
        {
            await entity.Add(client)
            .Context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> Update(Client client)=>
            await entity.Where(x=>x.Id== client.Id)
            .ExecuteUpdateAsync(s=> 
            s.SetProperty(x=>x.Application,client.Application)
            .SetProperty(x=>x.ClientId,client.ClientId)
            .SetProperty(x=>x.ClientName,client.ClientName)
            .SetProperty(x=>x.Updated,DateTime.UtcNow)
            .SetProperty(x=>x.UpdateBy,client.UpdateBy)
            .SetProperty(x=>x.TokenExpiration,client.TokenExpiration)
            .SetProperty(x=>x.RequireSecret,client.RequireSecret)
            .SetProperty(x=>x.MaxinumnAccess,client.MaxinumnAccess)
            )
            .GeAsync(0);

        public async Task<bool> Delete(long id, IDbConnection warehouseConnection, IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(TableConstraint.Client)
            .Where(nameof(Client.Id), id)
            .DeleteAsync(transaction)
            .GeAsync(0);



        public async Task<long> GetUnsafeClient() =>
            await entity.CountAsync(x => !x.RequireSecret);

        public async Task<long> GetIncompleteClient() =>
            await entity.CountAsync(x =>
            (x.RequireSecret && !context.ClientSecret.Any(y=>y.Client == x.Id)) ||
            (string.IsNullOrEmpty( x.Application)) ||
            !context.AllowedObject.Any(z=>z.Client==x.Id)
            );

        public async Task<long[]> ListUnSafeClientId() =>
            await entity.Where(x => !x.RequireSecret)
            .Select(s => s.Id)
            .ToArrayAsync();

        public async Task<long[]> ListIncompleteClientId() =>
             await entity.Where(x =>
            (x.RequireSecret && !context.ClientSecret.Any(y => y.Client == x.Id)) ||
            (string.IsNullOrEmpty(x.Application)) ||
            !context.AllowedObject.Any(z => z.Client == x.Id)
            )
            .Select(s => s.Id)
            .ToArrayAsync();
    }
}
