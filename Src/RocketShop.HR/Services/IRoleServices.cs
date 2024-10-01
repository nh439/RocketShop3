using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IRoleServices
    {
        Task<Either<Exception, bool>> Create(Role role);
        Task<Either<Exception, bool>> Update(Role role, IEnumerable<string>? AssignUserList = null);
        Task<Either<Exception, bool>> Delete(int roleId);
        Task<Either<Exception, int>> SetUserByRole(int roleId, string[] userIds);
        Task<Either<Exception, int>> SetRoleByUser(string userId, int[] roleIds);
        Task<Either<Exception, List<Role>>> ListRoles(string? searchName = null, int? page = null, int per = 20);
        Task<Either<Exception, List<Role>>> ListRoleByUsers(string userId, int? page = null, int per = 20);
        Task<Either<Exception, Option<Role>>> GetRole(int roleId);
        Task<Either<Exception, int>> RevokeAllRolesByUser(string userId);
        Task<Either<Exception, int>> GetCount(string? searchQuery = null);
        Task<Either<Exception, int>> GetLastpage(string? searchQuery = null, int per = 20);
        Task<Either<Exception, List<UserView>>> ListUserByRole(int roleId);
    }
    public class RoleServices(ILogger<RoleServices> logger,
        IConfiguration configuration,
        RoleRepository roleRepository,
        UserRoleRepository userRoleRepository,
        UserRepository userRepository) :
        BaseServices<RoleServices>("Role Service", logger, new NpgsqlConnection(configuration.GetIdentityConnectionString())), IRoleServices
    {
        public async Task<Either<Exception, bool>> Create(Role role) =>
            await InvokeServiceAsync(async () => await roleRepository.Create(role));

        public async Task<Either<Exception, bool>> Update(Role role,IEnumerable<string>? AssignUserList = null) =>
            await InvokeDapperServiceAsync(async connection => {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var result =  await roleRepository.Update(role, connection,transaction);
                if(!result)
                {
                    transaction.Rollback();
                    throw new Exception("Error While Update Role");    
                }
                if (AssignUserList.HasData())
                {
                    await userRoleRepository.TurncateUserByRole(role.Id, connection, transaction);
                    await userRoleRepository.AddUserByRole(role.Id, AssignUserList!.ToArray(), connection, transaction);
                }
                transaction.Commit();
                connection.Close();
                return true;
                });

        public async Task<Either<Exception, bool>> Delete(int roleId) =>
            await InvokeDapperServiceAsync(async connection =>
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var relatedUsers = await userRoleRepository.GetUserIdByRole(roleId, connection, transaction);
                await userRoleRepository.RemoveUsersByRole(roleId, relatedUsers, connection, transaction);
                var result = await roleRepository.Delete(roleId, connection, transaction);
                transaction.Commit();
                return result;
            });

        public async Task<Either<Exception, int>> SetUserByRole(int roleId, string[] userIds) =>
            await InvokeDapperServiceAsync(async con =>
            {
                con.Open();
                using var transaction = con.BeginTransaction();
                await userRoleRepository.RemoveUsersByRole(roleId, userIds, con, transaction);
                var result = await userRoleRepository.AddUserByRole(roleId, userIds, con, transaction);
                transaction.Commit();
                con.Close();
                return result;
            });

        public async Task<Either<Exception, int>> SetRoleByUser(string userId, int[] roleIds) =>
            await InvokeDapperServiceAsync(async con =>
            {
                con.Open();
                using var transaction = con.BeginTransaction();
                var deletedResult = await userRoleRepository.RevokeRoles(userId, con, transaction);
                var result = await userRoleRepository.AddRoleByUser(userId, roleIds, con, transaction);
                transaction.Commit();
                con.Close();
                return result;
            });

        public async Task<Either<Exception, List<Role>>> ListRoles(string? searchName = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await roleRepository.GetRoles(searchName, page, per));

        public async Task<Either<Exception, List<Role>>> ListRoleByUsers(string userId, int? page = null, int per = 20) =>
            await InvokeDapperServiceAsync(async con =>
            {
                var roleIds = await userRoleRepository.GetRoleIdByUsers(userId, con);
                return await roleRepository.GetIdIn(roleIds, page, per);
            });

        public async Task<Either<Exception, Option<Role>>> GetRole(int roleId) =>
            await InvokeServiceAsync(async () => await roleRepository.GetById(roleId));

        public async Task<Either<Exception, int>> RevokeAllRolesByUser(string userId) =>
            await InvokeDapperServiceAsync(async con => await userRoleRepository.RevokeRoles(userId, con));

        public async Task<Either<Exception, int>> GetCount(string? searchQuery = null) =>
            await InvokeServiceAsync(async () => await roleRepository.GetCount(searchQuery));

        public async Task<Either<Exception, int>> GetLastpage(string? searchQuery = null, int per = 20) =>
            await InvokeServiceAsync(async () => await roleRepository.GetLastpage(searchQuery, per));

        public async Task<Either<Exception, List<UserView>>> ListUserByRole(int roleId) =>
            await InvokeDapperServiceAsync(async con =>
            {
                var userIdList = await userRoleRepository.GetUserIdByRole(roleId, con);
                return await userRepository.ListUserIn(userIdList);
            });



    }
}
