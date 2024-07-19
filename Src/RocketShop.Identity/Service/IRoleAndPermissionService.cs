using Dapper;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;

namespace RocketShop.Identity.Service
{
    public interface IRoleAndPermissionService
    {
        Task<Either<Exception, bool>> PermissionCheckByUserId(string userId, string permissionName);
        Task<Either<Exception, List<Role>>> GetRoleByUserId(string userId);
        Task<Either<Exception, List<Role>>> GetRoles(string? searchRoleName);
        Task<Either<Exception, Role>> GerRole(int roleId);
    }
    public class RoleAndPermissionService(Serilog.ILogger logger, 
        IConfiguration configuration,
        IdentityContext context) 
        : BaseServices("Role And Permission Service", logger, new NpgsqlConnection(configuration.GetIdentityConnectionString())), IRoleAndPermissionService
    {


        public async Task<Either<Exception, bool>> PermissionCheckByUserId(string userId,string permissionName) =>
            await InvokeDapperServiceAsync(async con =>
            {
                var sql = @$"select count(*) from ""Roles"" r  inner join ""UserRoles"" ur
on r.""Id"" = ur.""RoleId"" where ur.""UserId""=@userId and ""{permissionName}"" = True;";
                var result = await con.QuerySingleOrDefaultAsync<int>(sql, new { userId = userId });
                return result.Ge(0);
            });

        public async Task<Either<Exception, List<Role>>> GetRoleByUserId(string userId) =>
            await InvokeServiceAsync(async () =>
            {
                var roleId = context.UserRole.Where(x => x.UserId == userId).Select(s => s.RoleId);
                return await context.Role.Where(x => roleId.Contains(x.Id))
                .ToListAsync();
            });

        public async Task<Either<Exception, List<Role>>> GetRoles(string? searchRoleName) =>
            await InvokeServiceAsync(async () => await context.Role
            .Where(x => string.IsNullOrEmpty(searchRoleName) || x.RoleName.Contains(searchRoleName))
            .ToListAsync());

        public async Task<Either<Exception, Role>> GerRole(int roleId) =>
            await InvokeServiceAsync(async () => await context.Role.FirstOrDefaultAsync(x => x.Id == roleId));
    }
}
