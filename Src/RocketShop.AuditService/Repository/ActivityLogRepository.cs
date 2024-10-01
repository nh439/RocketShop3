using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.AuditLog;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.AuditService.Repository
{
    public class ActivityLogRepository(AuditLogContext context)
    {
        readonly DbSet<ActivityLog> entity = context.ActivityLog;

        public async Task<bool> Create(ActivityLog log) =>
            await entity.Add(log).Context.SaveChangesAsync().GeAsync(0);

        public async Task<List<ActivityLog>> GetActivityLogs(string? searchKeyword, int? page, int per) =>
           searchKeyword.HasMessage() ?
            await entity
            .Where(x=> x.ServiceName.Contains(searchKeyword!) ||
            x.ActorName.Contains(searchKeyword!) ||
            x.Division.Contains(searchKeyword!))
            .UsePaging(page, per)
            .ToListAsync() :
        await entity.UsePaging(page, per)
            .ToListAsync();

        public async Task<List<ActivityLog>> GetByService(string division,string serviceName, int? page, int per) =>
            await entity.Where(x => x.ServiceName == serviceName && x.Division == division)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<List<ActivityLog>> GetByUserId(string userId, int? page, int per) =>
            await entity.Where(x => x.Actor == userId)
            .UsePaging(page, per)
            .ToListAsync();
    }
}
