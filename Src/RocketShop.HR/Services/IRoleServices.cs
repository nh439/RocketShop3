using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IRoleServices
    {
    }
    public class RoleServices(Serilog.ILogger logger,
        IConfiguration configuration,
        RoleRepository roleRepository,
        UserRoleRepository userRoleRepository) :
        BaseServices("Role Service",logger,new NpgsqlConnection(configuration.GetIdentityConnectionString())) , IRoleServices
    {
        public async Task<Either<Exception, bool>> Create(Role role) =>
            await InvokeServiceAsync(async () => await roleRepository.Create(role));

        public async Task<Either<Exception, bool>> Update(Role role) =>
            await InvokeDapperServiceAsync(async connection => await roleRepository.Update(role,connection));

        public async Task<Either<Exception, bool>> Delete(int roleId) =>
            await InvokeDapperServiceAsync(async connection =>
            {
                connection.Open();
                using var transaction = connection.BeginTransaction();
                var relatedUsers = await userRoleRepository.GetUserIdByRole(roleId, connection, transaction);
                await userRoleRepository.RemoveUsersByRole(roleId, relatedUsers, connection,transaction);
                var result= await roleRepository.Delete(roleId, connection, transaction);
                transaction.Commit();
                return result;
            });
    }
}
