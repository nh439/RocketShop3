using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace RocketShop.HR.Repository
{
    public class UserReportRepository(IdentityContext context)
    {
        public async Task<List<EmployeeDataReport>> ListEmployeeDataReport(
            string? keyword = null,
            int? page = null,
        int per = 100) =>
             await GenerateReportQuery(keyword)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<int> GetEmployeeDataReportCount(string? keyword = null) =>
            await GenerateReportQuery(keyword).CountAsync();

        public async Task<int> GetEmployeeDataReportLastpage(string? keyword = null, int per = 100) =>
            await GenerateReportQuery(keyword)
            .GetLastpageAsync();



        IQueryable<EmployeeDataReport> GenerateReportQuery(string? keyword)
        {
            var query = (from user in context.Users
                         join information in context.UserInformation
                         on user.Id equals information.UserId
                         into userInformation
                         from ui in userInformation.DefaultIfEmpty()
                         select new
                         {
                             user = user,
                             info = ui,
                             hasInfo = ui != default
                         }
                       )
                       .Select(s => new EmployeeDataReport
                       {
                           Active = !s.user.Resigned,
                           CreateByName = context.Users.Where(x => x.Id == s.user.Id).Select(us => $"{us.Firstname} {us.Surname}").FirstOrDefault(),
                           Firstname = s.user.Firstname,
                           Surname = s.user.Surname,
                           CreateDate = s.user.CreateDate,
                           Department = s.hasInfo ? s.info.Department : null,
                           Email = s.user.Email!,
                           EmployeeCode = s.user.EmployeeCode,
                           HasFinancial = context.UserFinancal.Any(a => a.UserId == s.user.Id),
                           HasInformation = s.hasInfo,
                           UserId = s.user.Id,
                           ManagerName = s.hasInfo ? context.Users.Where(x => x.Id == s.info.ManagerId).Select(us => $"{us.Firstname} {us.Surname}").FirstOrDefault() : null,
                           Position = s.hasInfo ? s.info.CurrentPosition : null,
                           Prefix = s.user.Prefix
                       });

            if (keyword.HasMessage())
                query = query.Where(x =>
                x.EmployeeCode.Contains(keyword!) ||
                x.Firstname.Contains(keyword!) ||
                x.Surname.Contains(keyword!) ||
                x.Department.Contains(keyword!) ||
                x.Position.Contains(keyword!) ||
                x.Email.Contains(keyword!) ||
                x.Prefix.Contains(keyword!)
                );
            return query;
        }
    }
}
