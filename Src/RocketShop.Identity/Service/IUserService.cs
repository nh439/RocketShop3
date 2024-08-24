using LanguageExt;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;

namespace RocketShop.Identity.Service
{
    public interface IUserService
    {
        Task<Either<Exception, List<UserView>>> GetUserList(int? page, int per = 20);
        Task<Either<Exception, List<UserView>>> GetUserList(string keyword, int? page, int per = 20);
        Task<Either<Exception, int>> GetAllUserCount(string? keyword = null);
        Task<Either<Exception, int>> GetLastPage(string? keyword = null, int per = 20);
        Task<Either<Exception, UserView>> GetById(string userId);
        Task<Either<Exception, UserView>> GetByEmpCode(string employeeCode);
        Task<Either<Exception, UserView>> GetByEmail(string email);
    }
    public class UserService (IdentityContext context, ILogger<UserService> logger)
        : BaseServices<UserService>("User Service", logger)
        ,IUserService
    {
        readonly DbSet<User> entity = context.Users;

        public async Task<Either<Exception, List<UserView>>> GetUserList(int? page, int per = 20) =>
            await InvokeServiceAsync(async () =>
            {
                var query = context.UserViews.OrderBy(x => x.EmployeeCode)
                .OrderByDescending(x=>x.Active);
                return page.HasValue ? 
                    await query.UsePaging(page.Value, per).ToListAsync() :
                    await query.ToListAsync();
            });

        public async Task<Either<Exception, List<UserView>>> GetUserList(string keyword, int? page, int per = 20) =>
            await InvokeServiceAsync(async () =>
            {
                var query = context.UserViews
                .Where(x => x.EmployeeCode.Contains(keyword) ||
                        string.Join(" ", x.Firstname, x.Surname).Contains(keyword))
                .OrderBy(x => x.EmployeeCode).OrderByDescending(x => x.Active);
                return page.HasValue ?
                   await query.UsePaging(page.Value, per).ToListAsync() :
                   await query.ToListAsync();
            });

        public async Task<Either<Exception, int>> GetAllUserCount(string? keyword = null) =>
            await InvokeServiceAsync(async () =>
            {
                var query = context.UserViews.AsQueryable();
                if (keyword.HasMessage())
                    query = query.Where(x => x.EmployeeCode.Contains(keyword!) ||
                        string.Join(" ", x.Firstname, x.Surname).Contains(keyword!));
                return await query.CountAsync();
            });
        public async Task<Either<Exception, int>> GetLastPage(string? keyword = null, int per = 20) =>
            await InvokeServiceAsync(async () =>
            {
                var query = context.UserViews.AsQueryable();
                if (keyword.HasMessage())
                    query = query.Where(x => x.EmployeeCode.Contains(keyword!) ||
                        string.Join(" ", x.Firstname, x.Surname).Contains(keyword!));
                return await query.GetLastpageAsync(per);
            });

        public async Task<Either<Exception, UserView>> GetById(string userId) =>
            await InvokeServiceAsync(async () =>  await context.UserViews.FirstOrDefaultAsync(x => x.UserId == userId));

        public async Task<Either<Exception, UserView>> GetByEmpCode(string employeeCode) =>
            await InvokeServiceAsync(async () =>  await context.UserViews.FirstOrDefaultAsync(x => x.EmployeeCode == employeeCode));

        public async Task<Either<Exception, UserView>> GetByEmail(string email) =>
            await InvokeServiceAsync(async () => await context.UserViews.FirstOrDefaultAsync(x => x.Email == email));

    }
}
