﻿using Dapper;
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
            IDbTransaction? transaction = null) =>
            await identityConnection.ExecuteAsync(@$"insert into ""{table}"" (""{nameof(UserProvidentFund.UserId)}"",""{nameof(UserProvidentFund.Balance)}"",""{nameof(UserProvidentFund.Currency)}"")
values(@userId,@balance,@currency)
on conflict (""{nameof(UserProvidentFund.UserId)}"")
do update set ""{nameof(UserProvidentFund.Balance)}"" = ""{nameof(UserProvidentFund.Balance)}""+@balance;
", new {
                userId=userId,
                balance=secondOperand,
                currency=currency
            },transaction).EqAsync(1);

        public async Task<UserProvidentFund?> GetUserProvidentFund(string userId) =>
            await context.UserProvidentFund.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
