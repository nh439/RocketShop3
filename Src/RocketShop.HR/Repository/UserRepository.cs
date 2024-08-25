using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class UserRepository(IdentityContext context, UserManager<User> userManager)
    {
        readonly DbSet<User> userTable = context.Users;
        readonly DbSet<UserView> userView = context.UserViews;

        public async Task<IdentityResult> Create(User user) =>
            await userManager.CreateAsync(user);

        public async Task<IdentityResult> Update(User user) =>
            await userManager.UpdateAsync(user);

        public async Task<IdentityResult> Delete(User user) =>
           await userManager.DeleteAsync(user);

        public async Task<IdentityResult> ResetPassword(User user, string newPassword)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<Option<User>> FindById(string userId) =>
            await userManager.FindByIdAsync(userId);

        public async Task<Option<User>> FindByEmail(string userId) =>
            await userManager.FindByEmailAsync(userId);

        public async Task<Option<User>> FindByEmployeeCode(string employeeCode) =>
            await userTable.FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode);

        public async Task<List<UserView>> GetUsers(string? searchTerm = null, int? page = null, int pageSize = 20)
        {
            var query = userView.AsQueryable();
            if (searchTerm.HasMessage())
                query = SearchUser(query, searchTerm!);
            query = query.OrderBy(x => x.EmployeeCode)
                .OrderByDescending(x=>x.Active);
            if (page.HasValue)
                query = query.UsePaging(page.Value, pageSize);
            return await query.ToListAsync();
        }
        public async Task<List<UserView>> GetByDepartment(string department, int? page = null, int pageSize = 20)
        {
            var query = userView
                .Where(x => x.Department == department)
                .OrderBy(x => x.EmployeeCode).AsQueryable();
            if (page.HasValue)
                query = query.UsePaging(page.Value, pageSize);
            return await query.ToListAsync();
        }

        public async Task<List<UserView>> GetByManager(string managerId, int? page = null, int pageSize = 20)
        {
            var query = userView
                .Where(x => x.ManagerId == managerId)
                .OrderBy(x => x.EmployeeCode).AsQueryable();
            if (page.HasValue)
                query = query.UsePaging(page.Value, pageSize);
            return await query.ToListAsync();
        }

        public async Task<int> GetCount(string? searchTerm= null) =>
            await SearchUser(userView, searchTerm)
            .CountAsync();

        public async Task<int> GetLastpage(string? searchTerm = null, int per = 20) =>
            await SearchUser(userView, searchTerm)
            .GetLastpageAsync(per);

        public async Task<int> GetCountByDepartment(string department) =>
            await userView.Where(x => x.Department == department)
            .CountAsync();

        public async Task<int> GetLastpageByDepartment(string department, int per = 20) =>
            await userView.Where(x => x.Department == department)
            .GetLastpageAsync(per);

        public async Task<int> GetCountByManager(string managerId) =>
                    await userView.Where(x => x.ManagerId == managerId)
                    .CountAsync();

        public async Task<int> GetLastpageByManager(string managerId, int per = 20) =>
            await userView.Where(x => x.ManagerId == managerId)
            .GetLastpageAsync(per);

        public async Task<IdentityResult> ResetPassword(string userId,string newPassword)
        {
            var user = (await FindById(userId)).Extract()!;
            if (!await userManager.HasPasswordAsync(user)) 
                return await userManager.AddPasswordAsync(user, newPassword);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return await userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<UserView?> GetUserViewById(string userId) =>
            await userView.FirstOrDefaultAsync(x => x.UserId == userId);

        public async Task<List<UserView>> ListUserIn(string[] userIds, string? searchKeyword = null, bool? isActive = null,int? take = null) =>
            await userView.Where(x=>userIds.Contains(x.UserId) && (isActive.IsNull() || isActive!.Value )
            && (searchKeyword.IsNullOrEmpty() || string.Join(" ", x.EmployeeCode, x.Prefix,x.Firstname,x.Surname).Contains(searchKeyword!) )
            )
            .UsePaging(take.HasValue ? 1 : null, take ?? 10)
            .ToListAsync();

        public async Task<List<UserView>> ListUserNOTIn(string[] userIds,string? searchKeyword = null, bool? isActive = null, int? take = null) =>
            await userView
            .Where(x=>!userIds.Contains(x.UserId) && (isActive.IsNull() || isActive!.Value)
            && (searchKeyword.IsNullOrEmpty() || string.Join(" ",x.EmployeeCode, x.Prefix, x.Firstname, x.Surname).ToLower().Contains(searchKeyword!.ToLower()))
            )
            .UsePaging(take.HasValue ? 1 : null,take ?? 10)
            .ToListAsync();

        public async Task<bool> LockedUser(string userId, DateTime lockUntil) =>
            await context.Users.Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.LockoutEnd, lockUntil)) > 0;

        public async Task<bool> UnlockUser(string userId) =>
            await context.Users.Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.LockoutEnd,l=> null)) > 0;

        IQueryable<UserView> SearchUser(IQueryable<UserView> query, string? searchTerm) =>
           searchTerm.HasMessage() ? query.Where(x => x.EmployeeCode.Contains(searchTerm!) ||
                x.Email.Contains(searchTerm!) ||
                x.Firstname.Contains(searchTerm!) ||
                x.Surname.Contains(searchTerm!) ||
                x.EmployeeCode.Contains(searchTerm!)) : query;

    }
}
