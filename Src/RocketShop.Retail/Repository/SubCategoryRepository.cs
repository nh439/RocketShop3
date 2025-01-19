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
    public class SubCategoryRepository(IDbContextFactory<RetailContext> factory, IConfiguration configuration)
    {
        readonly DbSet<SubCategory> entity = factory.CreateDbContext().SubCategories;
        readonly string tableName = TableConstraint.SubCategory;
        readonly bool EnabledSqlLogging = configuration.EnabledSqlLogging();

        public async Task<bool> Create(SubCategory subCategory, IDbConnection retailConnection, IDbTransaction? transaction = null) =>
           await retailConnection.CreateQueryStore(tableName, EnabledSqlLogging)
            .InsertAsync(subCategory, transaction, autoGenerateColumn: nameof(MainCategory.Id));

        public async Task<int> Creates(IEnumerable<SubCategory> subCategories,IDbConnection retailConnection,IDbTransaction? transaction = null)=>
            await retailConnection.CreateQueryStore(tableName, EnabledSqlLogging)
            .BulkInsertAsync(subCategories, transaction, autoGenerateColumn: nameof(MainCategory.Id));

        public async Task<bool> Update(SubCategory subCategory, IDbConnection retailConnection, IDbTransaction? transaction = null) =>
            await retailConnection.CreateQueryStore(tableName, EnabledSqlLogging)
            .Where(nameof(SubCategory.Id), subCategory.Id)
            .UpdateAsync(subCategory, transaction)
            .GeAsync(0);

        public async Task<bool> Delete(long id) =>
            await entity.Where(x => x.Id == id).ExecuteDeleteAsync().GeAsync(0);

        public async Task<int> Deletes(IEnumerable<long> ids) =>
            await entity.Where(x => ids.Contains(x.Id)).ExecuteDeleteAsync();

        public async Task<List<SubCategory>> ListSubCategories(string? search=null,int? page=null,int per =20)=>
            await entity.Where(x => search.IsNullOrEmpty() ||  (x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!)))
            .OrderByDescending(x => x.Id)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<List<SubCategory>> SubCategoriesByMainCategory(long mainCategoryId, string? search = null, int? page = null, int per = 20) =>
            await entity.Where(x => x.MainCategoryId == mainCategoryId && ( search.IsNullOrEmpty() || ( x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!))))
            .OrderByDescending(x => x.Id)
            .UsePaging(page, per)
            .ToListAsync();

        public async Task<SubCategory?> GetSubCategory(long id) =>
            await entity.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<int> GetCount(string? search = null) =>
            await entity.CountAsync(x => search.IsNullOrEmpty() || ( x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!) ));

        public async Task<int> GetLastPage(string? search = null, int per = 20) =>
            await entity.Where(x => search.IsNullOrEmpty() || ( x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!) ))
            .GetLastpageAsync(per);

        public async Task<int> GetCountByMainCategoryId(long mainCategoryId, string? search = null) =>
            await entity.CountAsync(x => x.MainCategoryId == mainCategoryId && (search.IsNullOrEmpty() ||( x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!) )));

        public async Task<int> GetLastpageByMainCategoryId(long mainCategoryId, string? search = null, int per = 20) =>
            await entity.Where(x => x.MainCategoryId == mainCategoryId && (search.IsNullOrEmpty() || ( x.NameEn.Contains(search!) || (x.NameTh ?? string.Empty).Contains(search!))))
            .GetLastpageAsync(per);

        public async Task<SubCategory?> GetPrimarySubCategory(long mainCategoryId) =>
            await entity.FirstOrDefaultAsync(x => x.MainCategoryId == mainCategoryId && x.Primary);

        public async Task<int> DeleteByMainCategory(long mainCategoryId) =>
            await entity.Where(x => x.MainCategoryId == mainCategoryId).ExecuteDeleteAsync();
    }
}
