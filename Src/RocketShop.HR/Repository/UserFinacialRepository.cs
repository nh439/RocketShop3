using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class UserFinacialRepository(IdentityContext context)
    {
        readonly string finacialTable = TableConstraint.UserFinancal,
            finacialView = TableConstraint.UserFinacialView;

        public async Task<List<UserFinancialView>> ListFinancialData(IEnumerable<string>? userIn = null, int? page = null, int per = 20) =>
            userIn.HasData() ?
            await context.UserFinancialView.Where(x => userIn!.Contains(x.UserId))
            .UsePaging(page, per)
            .ToListAsync() :
             await context.UserFinancialView
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<bool> CreateFinacialData(UserFinancal financal, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
           await identityConnection.CreateQueryStore(finacialTable)
            .InsertAsync(financal, transaction);

        public async Task<bool> UpdateFinacialData(UserFinancal financal, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(finacialTable)
            .Where(nameof(UserFinancal.UserId),financal.UserId)
            .UpdateAsync(financal,transaction).GeAsync(0);

        public async Task<bool> DeleteFinacialData(string userId, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(finacialTable)
            .Where(nameof(UserFinancal.UserId),userId)
            .DeleteAsync(transaction).GeAsync(0);

        public async Task<UserFinancal?> GetUserFinancal(string userId) =>
            await context.UserFinancal.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
