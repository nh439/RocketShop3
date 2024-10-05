using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class ProvidentRepository(IdentityContext context)
    {
        readonly string table = TableConstraint.UserProvidentFund;

        public async Task<bool> Create(UserProvidentFund userProvidentFund,IDbConnection identityConnection,IDbTransaction? transaction = null)=>
            await identityConnection.CreateQueryStore(table)
            .InsertAsync(userProvidentFund,transaction);

        public async Task<int> BulkCreateOrUpdate(IEnumerable<UserProvidentFund> userProvidentFunds, IDbConnection identityConnection, IDbTransaction? transaction = null) => 
            await identityConnection.ExecuteAsync(
                $@"insert into ""{table}"" 
                    values(@{nameof(UserProvidentFund.UserId)},@{nameof(UserProvidentFund.Balance)},@{nameof(UserProvidentFund.Currency)})
                on conflict(""{nameof(UserProvidentFund.UserId)}"") do nothing;",
                userProvidentFunds,
                transaction
                );


        public async Task<bool> Update(UserProvidentFund userProvidentFund, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(table)
            .Where(nameof(UserProvidentFund.UserId),userProvidentFund.UserId)
            .UpdateAsync(userProvidentFund,transaction).GeAsync(0);

        public async Task<bool> Delete(string userId, IDbConnection identityConnection, IDbTransaction? transaction = null)=>
            await identityConnection.CreateQueryStore(table)
            .Where(nameof(UserProvidentFund.UserId), userId)
            .DeleteAsync(transaction).GeAsync(0);

        public async Task<bool> AddProvidentFundValue(string userId,
            decimal secondOperand,
            string currency,
            IDbConnection identityConnection,
            IDbTransaction? transaction = null)
        {
            var query = identityConnection.CreateQueryStore(table);
            var condQuery = query.Where(nameof(UserProvidentFund.UserId), userId);
            var existsData = await condQuery.FetchOneAsync<UserProvidentFund>(transaction);
            if (existsData.IsSome)
            {
                var data = existsData.Extract();
                var newBalance = (data!.Balance) + secondOperand;
                return await condQuery.UpdateAsync(new
                {
                    Balance = newBalance.Le(0) ? 0 : newBalance
                }, transaction).GeAsync(0);
            }
            return await query.InsertAsync(new UserProvidentFund
            {
                Balance = secondOperand,
                Currency = currency,
                UserId = userId
            }, transaction);
                
        }
           

        public async Task<UserProvidentFund?> GetUserProvidentFund(string userId) =>
            await context.UserProvidentFund.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
