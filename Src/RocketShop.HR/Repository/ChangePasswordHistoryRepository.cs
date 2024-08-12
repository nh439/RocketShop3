using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;

namespace RocketShop.HR.Repository
{
    public class ChangePasswordHistoryRepository(IdentityContext context)
    {
        readonly DbSet<ChangePasswordHistory> entity = context.ChangePasswordHistory;
        public async Task<bool> CreateHistories(ChangePasswordHistory item) =>
            await entity.Add(item).Context.SaveChangesAsync().GeAsync(0);
    }
}
