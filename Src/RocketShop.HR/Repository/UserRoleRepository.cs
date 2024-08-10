using RocketShop.Database;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class UserRoleRepository
    {
        public async Task<string[]> GetUserIdByRole(int roleId, IDbConnection connection, IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.UserRole)
            .Where(nameof(UserRole.RoleId), roleId)
            .Select(nameof(UserRole.UserId))
            .ToArrayAsync<string>(transaction);

        public async Task<int[]> GetRoleIdByUsers(string userId, IDbConnection connection, IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.UserRole)
            .Where (nameof(UserRole.UserId), userId)
            .Select(nameof(UserRole.RoleId))
            .ToArrayAsync<int>(transaction);

        public async Task<int> AddRoleByUser(string userId,
            int[] roleIds,
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
           await connection.CreateQueryStore(TableConstraint.UserRole)
            .BulkInsertAsync(roleIds.Select(s => new UserRole
            {
                RoleId = s,
                UserId = userId
            }),
               transaction,
               200);

         public async Task<int> AddUserByRole(int roleId,
            string[] userIds,
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
           await connection.CreateQueryStore(TableConstraint.UserRole)
            .BulkInsertAsync(userIds.Select(s => new UserRole
            {
                RoleId = roleId,
                UserId = s
            }),
               transaction,
               200);

        public async Task<int> RemoveRoleByUser(string userId,
            int[] roleIds,
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.UserRole)
            .Where(x=>x.Where(nameof(UserRole.UserId),userId)
            .WhereIn(nameof(UserRole.RoleId),roleIds))
            .DeleteAsync(transaction, 200);

        public async Task<int> RemoveUsersByRole(int roleId,
            string[] userIds,
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.UserRole)
            .Where(x=>x.Where(nameof(UserRole.RoleId),roleId)
            .WhereIn(nameof(UserRole.UserId),userIds))
            .DeleteAsync(transaction, 200);

        public async Task<int> RevokeRoles(string userId, 
            IDbConnection connection,
            IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.UserRole)
            .Where(nameof(UserRole.UserId),userId)
            .DeleteAsync (transaction, 200);



    }
}
