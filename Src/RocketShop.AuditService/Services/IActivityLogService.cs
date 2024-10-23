using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RocketShop.AuditService.Domain;
using RocketShop.AuditService.Repository;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.AuditLog;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.AuditService.Services
{
    public interface IActivityLogService
    {
        Task<Either<Exception, bool>> Create(string division, string service, string logDetail);
        Task<Either<Exception, List<ActivityLog>>> GetActivityLogs(string? searchKeyword, int? page, int per=50);
        Task<Either<Exception, List<ActivityLog>>> GetByService(string division, string serviceName, int? page, int per=50);
        Task<Either<Exception, List<ActivityLog>>> GetByUserId(string userId, int? page, int per = 50);
        Task<Either<Exception, int>> GetCount(string? searchKeyword);
        Task<Either<Exception, int>> GetLastpage(string? searchKeyword, int per =50);
        Task<Either<Exception, int>> GetCount(ActivityLogAdvanceSearch advanceSearch);
        Task<Either<Exception, int>> GetLastpage(ActivityLogAdvanceSearch advanceSearch, int per=50);
        Task<Either<Exception, List<ActivityLog>>> GetActivityLogs(ActivityLogAdvanceSearch advanceSearch, int? page, int per = 50);
    }
    public class ActivityLogService(
        ILogger<ActivityLogService> logger,
        ActivityLogRepository repository,
        IdentityContext identityContext,
        IHttpContextAccessor accessor) : BaseServices<ActivityLogService>("Activity Service", logger),IActivityLogService 
    {
        public async Task<Either<Exception, bool>> Create(string division,string service,string logDetail) =>
            await InvokeServiceAsync(async () =>{
                var userId = accessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
                var email = accessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;
                var user = await identityContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
                var item = new ActivityLog
                {
                    Division = division,
                    Actor = userId,
                    ActorEmail = email,
                    LogDetail = logDetail,
                    ServiceName = service,
                    ActorName= $"{(user!.Prefix.HasMessage() ? user.Prefix : string.Empty)} {user.Firstname} {user.Surname}"
                    
                };
                return await repository.Create(item);
            });

        public async Task<Either<Exception, List<ActivityLog>>> GetActivityLogs(string? searchKeyword, int? page, int per=50) =>
            await InvokeServiceAsync(async () => await repository.GetActivityLogs(searchKeyword, page, per));

        public async Task<Either<Exception, List<ActivityLog>>> GetByService(string division,string serviceName, int? page, int per = 50) =>
            await InvokeServiceAsync(async () => await repository.GetByService(division,serviceName, page, per));

        public async Task<Either<Exception, List<ActivityLog>>> GetByUserId(string userId, int? page, int per = 50) =>
            await InvokeServiceAsync(async () => await repository.GetByUserId(userId, page, per));

        public async Task<Either<Exception, List<ActivityLog>>> GetActivityLogs(ActivityLogAdvanceSearch advanceSearch, int? page, int per=50)=>
            await InvokeServiceAsync(async () => await repository.GetActivityLogs(SpecifyLocate(advanceSearch), page, per));

        public async Task<Either<Exception, int>> GetCount(string? searchKeyword)=>
            await InvokeServiceAsync(async () =>await repository.GetCount(searchKeyword));

        public async Task<Either<Exception, int>> GetLastpage(string? searchKeyword, int per = 50) =>
            await InvokeServiceAsync(async () => await repository.GetLastpage(searchKeyword, per));

        public async Task<Either<Exception, int>> GetCount(ActivityLogAdvanceSearch advanceSearch)=>
            await InvokeServiceAsync(async () =>await repository.GetCount(SpecifyLocate(advanceSearch)));

        public async Task<Either<Exception, int>> GetLastpage(ActivityLogAdvanceSearch advanceSearch, int per =50) =>
            await InvokeServiceAsync(async () => await repository.GetLastpage(SpecifyLocate(advanceSearch), per));

        ActivityLogAdvanceSearch SpecifyLocate(ActivityLogAdvanceSearch advanceSearch)
        {
            if (advanceSearch.DateFrom.HasValue)
                advanceSearch.DateFrom = DateTime.SpecifyKind(advanceSearch.DateFrom.Value, DateTimeKind.Utc);
            if(advanceSearch.DateTo.HasValue)
                advanceSearch.DateTo = DateTime.SpecifyKind(advanceSearch.DateTo.Value, DateTimeKind.Utc);
            return advanceSearch;
        }
    }

}
