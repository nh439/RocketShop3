using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Retail;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using RocketShop.Shared.Extension;
using System.Data;

namespace RocketShop.Retail.Repository
{
    public class MainCategoryRepository(RetailContext context,IConfiguration configuration)
    {
        readonly DbSet<MainCategory>  entity = context.MainCategories;
        readonly string tableName = TableConstraint.MainCategory;
        readonly bool EnabledSqlLogging = configuration.EnabledSqlLogging();

        public async Task<MainCategory?> Create(MainCategory mainCategory,
            IDbConnection retailConnection,
            IDbTransaction? transaction = null
            ) =>
            await retailConnection.CreateQueryStore(tableName, EnabledSqlLogging)
            .InsertAndReturnItemAsync(mainCategory, transaction);

        public async Task<List<MainCategory>> Creates(IEnumerable<MainCategory> mainCategories, IDbConnection retailConnection,IDbTransaction? transaction = null)
        {
            List<MainCategory> returnValue = new List<MainCategory>();
            foreach (var mainCategory in mainCategories)
            {
                var result = await retailConnection.CreateQueryStore(tableName, EnabledSqlLogging)
                .InsertAndReturnItemAsync(mainCategory, transaction);
                if (result.IsNotNull())
                    returnValue.Add(result!);
            }
            return returnValue;
        }

        public async Task<bool> Update(MainCategory mainCategory,IDbConnection retailConnection,IDbTransaction? transaction = null) =>
           await retailConnection.CreateQueryStore(tableName,EnabledSqlLogging)
            .Where(nameof(MainCategory.Id),mainCategory.Id)
            .UpdateAsync(mainCategory, transaction)
            .GeAsync(0);

        public async Task<bool> Delete(long id) =>
            await entity.Where(x => x.Id == id).ExecuteDeleteAsync().GeAsync(0);

        public async Task<List<MainCategory>> ListMainCategories(
            string? search= null,
            int? page = null,
            int per = 20) =>
            search.HasMessage() ?
            await entity.Where(x => x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!))
            .UsePaging(page, per)
            .ToListAsync() :
            await entity.UsePaging(page, per).ToListAsync();

        public async Task<MainCategory?> GetMainCategory(long id) =>
            await entity.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> GetCount(string? search = null) =>
            await entity.CountAsync(x => x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!));

        public async Task<int> GetLastPage(string? search = null, int per = 20) =>
            await entity.Where(x => x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!))
            .GetLastpageAsync(per);
    }
}
