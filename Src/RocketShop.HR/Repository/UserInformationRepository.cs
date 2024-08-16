using LanguageExt;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class UserInformationRepository(IdentityContext context)
    {
        readonly DbSet<UserInformation> userInformationTbl = context.UserInformation;

        public async Task<bool> Create(UserInformation userInformation) =>
            await userInformationTbl.Add(userInformation)
            .Context
            .SaveChangesAsync()
            .GeAsync(0);

        public async Task<bool> Update(UserInformation userInformation) =>
             await userInformationTbl.Where(x => x.UserId == userInformation.UserId)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.BrithDay, userInformation.BrithDay)
            .SetProperty(t=>t.ResignDate,userInformation.ResignDate)
            .SetProperty(t=>t.StartWorkDate,userInformation.StartWorkDate)
            .SetProperty(t=>t.Department,userInformation.Department)
            .SetProperty(t=>t.CurrentPosition,userInformation.CurrentPosition)
            .SetProperty(t=>t.ManagerId,userInformation.ManagerId)
            .SetProperty(t=>t.Gender,userInformation.Gender)
            )
            .GeAsync(0);

        public async Task<bool> Delete(string userId)=>
            await userInformationTbl.Where (x => x.UserId == userId)
            .ExecuteDeleteAsync()
            .GeAsync(0);

        public async Task<bool> Delete(string userId,IDbConnection identityConnection,IDbTransaction? transaction = null)=>
            await identityConnection.CreateQueryStore(TableConstraint.UserInformation)
            .Where(nameof(UserInformation.UserId),userId)
            .DeleteAsync(transaction)
            .GeAsync(0);

        public async Task<Option< UserInformation>> GetInformation(string userId) =>
            await userInformationTbl.FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<bool> UpdatePosition(string userId,
            string newPosition,
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
            (await connection.CreateQueryStore(TableConstraint.UserInformation)
            .Where(nameof(UserInformation.UserId), userId)
            .UpdateAsync(new { CurrentPosition = newPosition })).Ge(0);

        public async Task<Option<string>> GetCurrentPosition(string userId, 
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.UserInformation)
            .Where(nameof(userId), userId)
            .Select(nameof(UserInformation.UserId))
            .FetchOneAsync<string>(transaction);
    }
}
