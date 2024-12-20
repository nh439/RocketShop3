using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Retail;
using RocketShop.Framework.Services;
using RocketShop.Retail.Repository;

namespace RocketShop.Retail.Service
{
    public interface ICategoryServices
    {
        Task<Either<Exception, List<MainCategory>>> ListMainCategories(string? search = null, int? page = null, int per = 20);
        Task<Either<Exception, MainCategory>> GetMainCategory(long id);
        Task<Either<Exception, int>> GetCountMainCategory(string? search = null);
        Task<Either<Exception, int>> GetLastPageMainCategory(string? search = null, int per = 20);
        Task<Either<Exception, bool>> CreateMainCategory(MainCategory mainCategory);
        Task<Either<Exception, int>> CreateMainCategories(IEnumerable<MainCategory> mainCategories);
        Task<Either<Exception, bool>> UpdateMainCategory(MainCategory mainCategory);
        Task<Either<Exception, bool>> DeleteMainCategory(long id);
        Task<Either<Exception, List<SubCategory>>> ListSubCategories(string? search = null, int? page = null, int per = 20);
        Task<Either<Exception, List<SubCategory>>> ListSubCategoriesByMainCategory(long mainCategoryId, string? search = null, int? page = null, int per = 20);
        Task<Either<Exception, SubCategory>> GetSubCategory(long id);
        Task<Either<Exception, int>> GetCountSubCategory(string? search = null);
        Task<Either<Exception, int>> GetLastPageSubCategory(string? search = null, int per = 20);
        Task<Either<Exception, int>> GetCountSubCategoryByMainCategoryId(long mainCategoryId, string? search = null);
        Task<Either<Exception, int>> GetLastPageSubCategoryByMainCategoryId(long mainCategoryId, string? search = null, int per = 20);
        Task<Either<Exception, bool>> CreateSubCategory(SubCategory subCategory);
        Task<Either<Exception, int>> CreateSubCategories(IEnumerable<SubCategory> subCategories);
        Task<Either<Exception, bool>> UpdateSubCategory(SubCategory subCategory);
        Task<Either<Exception, bool>> DeleteSubCategory(long id);
        Task<Either<Exception, int>> DeleteSubCategories(IEnumerable<long> ids);
    }
    public class CategoryServices(
        ILogger<CategoryServices> logger,
        IConfiguration configuration,
        MainCategoryRepository mainCategoryRepository,
        SubCategoryRepository subCategoryRepository
        ) : BaseServices<CategoryServices>("Category Services", logger,new NpgsqlConnection(configuration.GetRetailConnectionString())),ICategoryServices
    {
        public async Task<Either<Exception,bool>> CreateMainCategory(MainCategory mainCategory) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.Create(mainCategory));

        public async Task<Either<Exception, int>> CreateMainCategories(IEnumerable<MainCategory> mainCategories) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.Creates(mainCategories));

        public async Task<Either<Exception, bool>> UpdateMainCategory(MainCategory mainCategory) =>
            await InvokeDapperServiceAsync(async retailConnection => await mainCategoryRepository.Update(mainCategory, retailConnection));

        public async Task<Either<Exception, bool>> DeleteMainCategory(long id) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.Delete(id));

        public async Task<Either<Exception,List<MainCategory>>> ListMainCategories(string? search = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.ListMainCategories(search, page, per));

        public async Task<Either<Exception, MainCategory>> GetMainCategory(long id) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.GetMainCategory(id));

        public async Task<Either<Exception, int>> GetCountMainCategory(string? search = null) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.GetCount(search));

        public async Task<Either<Exception, int>> GetLastPageMainCategory(string? search = null, int per = 20) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.GetLastPage(search, per));

        public async Task<Either<Exception, bool>> CreateSubCategory(SubCategory subCategory) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.Create(subCategory));

        public async Task<Either<Exception, int>> CreateSubCategories(IEnumerable<SubCategory> subCategories) =>
            await InvokeDapperServiceAsync(async retailConnection => await subCategoryRepository.Creates(subCategories, retailConnection));

        public async Task<Either<Exception, bool>> UpdateSubCategory(SubCategory subCategory) =>
            await InvokeDapperServiceAsync(async retailConnection => await subCategoryRepository.Update(subCategory, retailConnection));

        public async Task<Either<Exception, bool>> DeleteSubCategory(long id) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.Delete(id));

        public async Task<Either<Exception, int>> DeleteSubCategories(IEnumerable<long> ids) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.Deletes(ids));

        public async Task<Either<Exception, List<SubCategory>>> ListSubCategories(string? search = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.ListSubCategories(search, page, per));

        public async Task<Either<Exception, List<SubCategory>>> ListSubCategoriesByMainCategory(long mainCategoryId, string? search = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.SubCategoriesByMainCategory(mainCategoryId, search, page, per));

        public async Task<Either<Exception, SubCategory>> GetSubCategory(long id) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.GetSubCategory(id));

        public async Task<Either<Exception, int>> GetCountSubCategory(string? search = null) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.GetCount(search));

        public async Task<Either<Exception, int>> GetLastPageSubCategory(string? search = null, int per = 20) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.GetLastPage(search, per));

        public async Task<Either<Exception, int>> GetCountSubCategoryByMainCategoryId(long mainCategoryId, string? search = null) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.GetCountByMainCategoryId(mainCategoryId, search));

        public async Task<Either<Exception, int>> GetLastPageSubCategoryByMainCategoryId(long mainCategoryId, string? search = null, int per = 20) =>
            await InvokeServiceAsync(async () => await subCategoryRepository.GetLastpageByMainCategoryId(mainCategoryId, search, per));
    }
}
