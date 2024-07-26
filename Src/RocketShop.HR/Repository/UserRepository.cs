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
    public class UserRepository(IdentityContext context,UserManager<User> userManager)
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
            return await userManager.ResetPasswordAsync(user,token,newPassword);
        }

        public async Task<Option<User>> FindById(string userId) =>
            await userManager.FindByIdAsync(userId);

        public async Task<Option<User>> FindByEmployeeCode(string employeeCode) =>
            await userTable.FirstOrDefaultAsync(x=>x.EmployeeCode == employeeCode);

        public async Task<Option<User>> FindByEmail(string email) =>
            await userManager.FindByEmailAsync(email);

        public async Task<List<UserView>> GetUsers(string? searchTerm=null,int? page= null, int pageSize= 20)
        {
            var query = userView.AsQueryable();
            if (searchTerm.HasMessage())
                query = SearchUser(query, searchTerm!);
            query = query.OrderBy(x => x.EmployeeCode);
            if(page.HasValue)
                query = query.UsePaging(page.Value, pageSize);
            return await query.ToListAsync();
        }
        public async Task<List<UserView>> GetByDepartment(string department, int? page = null, int pageSize = 20)
        {
            var query = userView
                .Where(x=>x.Department==department)
                .OrderBy(x => x.EmployeeCode).AsQueryable();
            if(page.HasValue)
                query = query.UsePaging(page.Value, pageSize);
            return await query.ToListAsync();
        }
         
        public async Task<List<UserView>> GetByManager(string managerId, int? page = null, int pageSize = 20)
        {
            var query = userView
                .Where(x=>x.ManagerId==managerId)
                .OrderBy(x => x.EmployeeCode).AsQueryable();
            if(page.HasValue)
                query = query.UsePaging(page.Value, pageSize);
            return await query.ToListAsync();
        }

        public async Task<int> GetCount(string searchTerm)=>
            await SearchUser(userView,searchTerm)
            .CountAsync();

        public async Task<int> GetLastpage(string searchTerm, int per = 20) =>
            await SearchUser(userView, searchTerm)
            .GetLastpageAsync(per);



        IQueryable<UserView> SearchUser(IQueryable<UserView> query,string searchTerm) =>
            query.Where(x => x.EmployeeCode.Contains(searchTerm!) ||
                x.Email.Contains(searchTerm!) ||
                x.Firstname.Contains(searchTerm!) ||
                x.Surname.Contains(searchTerm!) ||
                x.EmployeeCode.Contains(searchTerm!));


    }
}
