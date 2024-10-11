using LanguageExt;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;
using System.Linq;

namespace RocketShop.HR.Repository
{
    public class RoleRepository(IdentityContext context)
    {
        readonly DbSet<Role> entity = context.Role;

        public async Task<bool> Create(Role role) =>
            await entity.Add(role)
            .Context.SaveChangesAsync().GeAsync(0);

        public async Task<bool> Update(Role role, IDbConnection connection, IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.Role)
            .Where(nameof(Role.Id), role.Id)
            .UpdateAsync(role, transaction)
            .GeAsync(0);

        public async Task<bool> Delete(int roleId, IDbConnection connection, IDbTransaction? transaction = null) =>
            await connection.CreateQueryStore(TableConstraint.Role)
            .Where(nameof(Role.Id), roleId)
            .DeleteAsync(transaction)
            .GeAsync(0);

        public async Task<List<Role>> GetRoles(string? searchName = null,
            int? page = null,
            int per = 20) =>
            await SearchQuery(searchName)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<Option< Role>> GetById(int roleId) =>
            await entity.FirstOrDefaultAsync(x => x.Id == roleId);

        public async Task<List<Role>> GetIdIn(int[] roleIds,
            int? page = null,
            int per = 20) =>
            await entity.Where(x => roleIds.Contains(x.Id))
            .UsePaging(page,per)
            .ToListAsync();
            
        public async Task<int> GetCount(string? searchName = null)=>
            await SearchQuery(searchName)
            .CountAsync();

        public async Task<int> GetLastpage(string? searchName,int per =20)=>
            await SearchQuery(searchName)
            .GetLastpageAsync();

        IQueryable<Role> SearchQuery(string? searchName) =>
            entity.Where(x => !searchName.HasMessage() || x.RoleName.ToLower().Contains(searchName!.ToLower()));

        public async Task<List<(string, List<Role>)>> GetRoleByUserIdRange(IEnumerable<string> userIdRange)
        {
            var roleIdQuery = context.UserRole.Where(x => userIdRange.Contains(x.UserId));
            List < (string, List<Role>)> returnValue = new List<(string, List<Role>)>();
            foreach (var userId in userIdRange)
            {
                var relatedRole = roleIdQuery.Where(x=>x.UserId == userId).Select(s=>s.RoleId);
                var roles =  await context.Role.Where(x => relatedRole.Contains(x.Id))
               .ToListAsync();
                returnValue.Add(new(userId, roles));
            }
           return returnValue;
        }
         
    }
}
