using Dapper;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using System.Data;

namespace RocketShop.Identity.Service
{
    public interface IRoleAndPermissionService
    {
        Task<Either<Exception, bool>> PermissionCheckByUserId(string userId, string permissionName);
        Task<Either<Exception, List<Role>>> GetRoleByUserId(string userId);
        Task<Either<Exception, List<Role>>> GetRoles(string? searchRoleName);
        Task<Either<Exception, Role>> GerRole(int roleId);
        Task<Either<Exception, List<string>>> GetAuthoriozedPermissionList(string userId);
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

        public async Task<Either<Exception, List< string>>> GetAuthoriozedPermissionList(string userId) =>
            await InvokeDapperServiceAsync(async connection =>
            {
                if (connection.State != ConnectionState.Open) 
                    connection.Open();
                List<string> returnValue = new List<string>();
                var hasRole = await connection.CreateQueryStore(TableConstraint.UserRole)
                .Where(nameof(UserRole.UserId), userId)
                .Select(nameof(UserRole.RoleId))
                .FetchAsync<int>();
                if (!hasRole.HasData()) return new List<string>();
                foreach(var role in hasRole )
                    {
                    var permission = await GetAllowedPermissionByRole(connection, role);
                    returnValue.AddRange(permission);
                }
                return returnValue.Distinct().ToList();
            });

        async Task<List<string>> GetAllowedPermissionByRole(IDbConnection connection,int roleId)
        {
            var roleOpt = await connection.CreateQueryStore(TableConstraint.Role)
                .Where(nameof(Role.Id), roleId)
                .FetchOneAsync<Role>();
            var type = typeof(Role);
            var props = type.GetProperties();
            if (roleOpt.IsNone) return new List<string>();
            var role = roleOpt.Extract();
            List<string> permissions = new List< string>();
            props.HasDataAndForEach(prop =>
            {
                prop.If(x => x!.PropertyType == typeof(bool), x =>
                {
                    var value = (bool)prop.GetValue(role)!;
                    if (value)
                        permissions.Add(x!.Name);
                });
            });
            return permissions;
        }
    }
}
