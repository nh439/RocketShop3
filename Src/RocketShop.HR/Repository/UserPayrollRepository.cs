using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class UserPayrollRepository(IdentityContext context)
    {
        const string tableName = TableConstraint.UserPayroll;
        readonly DbSet<UserPayroll> entity = context.UserPayroll;

        public async Task<List<UserPayroll>> GetUserPayrolls(int? page = null, int per = 20) =>
            await entity
            .OrderByDescending(x => x.PayrollDate)
            .OrderBy(x => x.Cancelled)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<List<UserPayroll>> GetByUserIdRange(IEnumerable<string> userIdRange, int? page = null, int per = 20) =>
            await entity.Where(x => userIdRange.Contains(x.UserId))
             .OrderByDescending(x => x.PayrollDate)
            .OrderBy(x => x.Cancelled)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<List<UserPayroll>> GetByUserId(string userId, int? page = null, int per = 20) =>
            await entity.Where(x => x.UserId == userId)
             .OrderByDescending(x => x.PayrollDate)
            .OrderBy(x => x.Cancelled)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<UserPayroll?> GetUserPayroll(string payrollId) =>
            await entity.FirstOrDefaultAsync(x => x.PayRollId == payrollId);

        public async Task<bool> Create(UserPayroll userPayroll, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .InsertAsync(userPayroll);

        public async Task<int> BulkCreate(IEnumerable<UserPayroll> userPayrolls, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .BulkInsertAsync(userPayrolls, transaction);

        public async Task<bool> Update(UserPayroll userPayroll, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .UpdateAsync(userPayroll, transaction)
            .GeAsync(0);

        public async Task<bool> Delete(string payrollId, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .Where(nameof(UserPayroll), payrollId)
            .DeleteAsync(transaction)
            .GeAsync(0);

        public async Task<bool> CancelPayroll(string payrollId,string? cancelReason, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .Where(nameof(UserPayroll.PayRollId), payrollId)
            .UpdateAsync(new
            {
                Cancelled = true,
                CancelledReason = cancelReason
            }, transaction)
            .GeAsync(0);

        public async Task<int> GetCount(params string[]? userIdRange) =>
            userIdRange.HasData() ?
            await entity.Where(x => userIdRange!.Contains(x.UserId))
            .CountAsync() :
            await entity.CountAsync();

        public async Task<int> GetLastPage(int per,params string[]? userIdRange) =>
            userIdRange.HasData() ?
            await entity.Where(x => userIdRange!.Contains(x.UserId))
            .GetLastpageAsync(per) :
            await entity.GetLastpageAsync(per);
    }
}
