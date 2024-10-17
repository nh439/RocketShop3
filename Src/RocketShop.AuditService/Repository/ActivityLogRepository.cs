using DocumentFormat.OpenXml.Spreadsheet;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using RocketShop.AuditService.Domain;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.AuditLog;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
           await GenerateLogQuery(searchKeyword)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<List<ActivityLog>> GetActivityLogs(ActivityLogAdvanceSearch advanceSearch, int? page, int per) =>
           await AdvanceSearchQuery(advanceSearch)
            .UsePaging(page, per)
            .ToListAsync();


        public async Task<List<ActivityLog>> GetByService(string division, string serviceName, int? page, int per) =>
            await entity.Where(x => x.ServiceName == serviceName && x.Division == division)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<List<ActivityLog>> GetByUserId(string userId, int? page, int per) =>
            await entity.Where(x => x.Actor == userId)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<int> GetCount(string? searchKeyword) =>
            await GenerateLogQuery(searchKeyword).CountAsync();

        public async Task<int> GetLastpage(string? searchKeyword, int per) =>
            await GenerateLogQuery(searchKeyword)
            .GetLastpageAsync(per);

        public async Task<int> GetCount(ActivityLogAdvanceSearch advanceSearch) =>
            await AdvanceSearchQuery(advanceSearch)
            .CountAsync();

        public async Task<int> GetLastpage(ActivityLogAdvanceSearch advanceSearch, int per) =>
            await AdvanceSearchQuery(advanceSearch)
            .GetLastpageAsync(per);

        IQueryable<ActivityLog> AdvanceSearchQuery(ActivityLogAdvanceSearch advanceSearch)
        {
            if (advanceSearch.IsNull())
                return entity;
            var query = entity.AsQueryable();
            if (advanceSearch.DateFrom.HasValue)
                query = query.Where(x => x.LogDate.Date >= advanceSearch.DateFrom.Value.Date);
            if (advanceSearch.DateTo.HasValue)
                query = query.Where(x => x.LogDate.Date < advanceSearch.DateTo.Value.Date.AddDays(1));
            if (advanceSearch.Actor.HasMessage())
                query = query.Where(
                    x => x.Actor.ToLower() == advanceSearch.Actor!.ToLower() ||
                    x.ActorName.ToLower() == advanceSearch.Actor!.ToLower());
            if (advanceSearch.Email.HasMessage())
                query = query.Where(x => x.ActorEmail.ToLower() == advanceSearch.Email!.ToLower());
            if (advanceSearch.Division.HasMessage())
                query = query.Where(x => x.Division.ToLower() == advanceSearch.Division!.ToLower());
            if (advanceSearch.ServiceName.HasMessage())
                query = query.Where(x => x.ServiceName.ToLower() == advanceSearch.ServiceName!.ToLower());
            return query;
        }

        IQueryable<ActivityLog> GenerateLogQuery(string? searchKeyword) =>
            searchKeyword.HasMessage() ?
             entity
            .Where(x => x.ServiceName.Contains(searchKeyword!) ||
            x.ActorName.Contains(searchKeyword!) ||
            x.Division.Contains(searchKeyword!)) :
            entity;



    }
}
