using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        Task<Either<Exception, List<ActivityLog>>> GetActivityLogs(string? searchKeyword, int? page, int per);
        Task<Either<Exception, List<ActivityLog>>> GetByService(string division, string serviceName, int? page, int per);
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

        public async Task<Either<Exception, List<ActivityLog>>> GetActivityLogs(string? searchKeyword, int? page, int per) =>
            await InvokeServiceAsync(async () => await repository.GetActivityLogs(searchKeyword, page, per));

        public async Task<Either<Exception, List<ActivityLog>>> GetByService(string division,string serviceName, int? page, int per) =>
            await InvokeServiceAsync(async () => await repository.GetByService(division,serviceName, page, per));

    }

}
