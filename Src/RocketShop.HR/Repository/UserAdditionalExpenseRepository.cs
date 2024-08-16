using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class UserAdditionalExpenseRepository(IdentityContext context)
    {
        readonly string tableName = TableConstraint.UserAddiontialExpense;

        public async Task<List<UserAddiontialExpense>> ListAdditionalExpenseByUserId(string userId)=>
            await context.UserAddiontialExpense
            .Where(x => x.UserId == userId)
            .ToListAsync();

        public async Task<UserAddiontialExpense?> GetById(string id) =>
            await context.UserAddiontialExpense.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<bool> Create(
            UserAddiontialExpense addiontialExpense,
            IDbConnection identityConnection,
            IDbTransaction? transaction = null)=>
            await  identityConnection.CreateQueryStore(tableName)
            .InsertAsync(addiontialExpense,transaction);

        public async Task<int> Creates(IEnumerable<UserAddiontialExpense> userAddiontialExpenses,
            IDbConnection identityConnection,
            IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .BulkInsertAsync(userAddiontialExpenses,transaction);

        public async Task<bool> Update(UserAddiontialExpense userAddiontialExpense,
             IDbConnection identityConnection,
            IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(tableName)
            .Where(nameof(UserAddiontialExpense.Id), userAddiontialExpense.Id)
            .UpdateAsync(userAddiontialExpense,transaction)
            .GeAsync(0);

        public async Task<bool> Delete(string Id, 
            IDbConnection identityConnection,
            IDbTransaction? transaction = null) =>
           await identityConnection.CreateQueryStore(tableName)
            .Where(nameof(UserAddiontialExpense.Id),Id)
            .DeleteAsync(transaction)
            .GeAsync(0);

        public async Task<int> Deletes(string[] Id, 
            IDbConnection identityConnection,
            IDbTransaction? transaction = null) =>
           await identityConnection.CreateQueryStore(tableName)
            .WhereIn(nameof(UserAddiontialExpense.Id),Id)
            .DeleteAsync(transaction);
    }
}
