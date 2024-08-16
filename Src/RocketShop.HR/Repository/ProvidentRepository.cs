﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> Update(UserProvidentFund userProvidentFund, IDbConnection identityConnection, IDbTransaction? transaction = null) =>
            await identityConnection.CreateQueryStore(table)
            .Where(nameof(UserProvidentFund),userProvidentFund.UserId)
            .UpdateAsync(userProvidentFund,transaction).GeAsync(0);

        public async Task<bool> Delete(string userId, IDbConnection identityConnection, IDbTransaction? transaction = null)=>
            await identityConnection.CreateQueryStore(table)
            .Where(nameof(UserProvidentFund.UserId), userId)
            .DeleteAsync(transaction).GeAsync(0);

        public async Task<UserProvidentFund?> GetUserProvidentFund(string userId) =>
            await context.UserProvidentFund.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
