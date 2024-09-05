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

        public async Task<List<UserFinancialView>> ListFinancialData(string? searchName = null, int? page = null, int per = 20) =>
            searchName.HasMessage() ?
            await context.UserFinancialView.Where(x => searchName.IsNullOrEmpty() || x.EmployeeName.Contains(searchName!) || x.BankName.Contains(searchName!))
            .UsePaging(page, per)
            .ToListAsync() :
             await context.UserFinancialView
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<bool> CreateFinacialData(UserFinancal financal, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
           await identityConnection.CreateQueryStore(finacialTable)
            .InsertAsync(financal, transaction);

        public async Task<int> BulkCreateFinancialData(IEnumerable<UserFinancal> data, IDbConnection identityConnection, IDbTransaction? transaction = null)=>
            await identityConnection.CreateQueryStore(finacialTable)
            .BulkInsertAsync(data, transaction);

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

        public async Task<List<UserFinancal>> ListFinancialDataByUserIn(IEnumerable<string> userIdRange) =>
            await context.UserFinancal.Where(x => userIdRange.Contains(x.UserId)).ToListAsync();

        public async Task<int> ListFinacialCount(string? searchName = null) =>
            await context
            .UserFinancialView
            .Where(x=> searchName.IsNullOrEmpty() || x.EmployeeName.Contains(searchName!) || x.BankName.Contains(searchName!))
            .CountAsync();

        public async Task<int> ListFinacialLastpage(string? searchName = null,int per = 20)=>
            await context
            .UserFinancialView
            .Where(x => searchName.IsNullOrEmpty() || x.EmployeeName.Contains(searchName!) || x.BankName.Contains(searchName!))
            .GetLastpageAsync(per);

        public async Task<string[]> ListUsersWhenHasFinacialData() =>
            await context.UserFinancal
            .Select(s => s.UserId)
            .Distinct()
            .ToArrayAsync();

        public async Task<bool> UpdateProvidentFundRate(string userId,
            decimal newProvidentFundRate,
            decimal newTotalPayment,
            IDbConnection connection,
            IDbTransaction? transaction = null
            ) =>
            await connection.CreateQueryStore(finacialTable)
            .Where(nameof(UserFinancal.UserId), userId)
            .UpdateAsync(new
            {
                ProvidentFund = newProvidentFundRate,
                TotalPayment = newTotalPayment
            }).GeAsync(0);
       
    }
}

