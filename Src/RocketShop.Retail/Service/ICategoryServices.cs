using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Retail;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Retail.Model;
using RocketShop.Retail.Repository;
using static RocketShop.Retail.Model.MainCategoryExcelModelExtensions;

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
        Task<Either<Exception, List<MainCategoryExcelModelValidator>>> ValidateExcelData(IEnumerable<MainCategoryExcelModel> mainCategories);
    }
    public class CategoryServices(
        ILogger<CategoryServices> logger,
        IConfiguration configuration,
        MainCategoryRepository mainCategoryRepository,
        SubCategoryRepository subCategoryRepository
        ) : BaseServices<CategoryServices>("Category Services", logger, new NpgsqlConnection(configuration.GetRetailConnectionString())), ICategoryServices
    {
        public async Task<Either<Exception, bool>> CreateMainCategory(MainCategory mainCategory) =>
            await InvokeDapperServiceAsync(async retailConnection =>
                {
                    retailConnection.Open();
                    using var transaction = retailConnection.BeginTransaction();
                    var result = await mainCategoryRepository.Create(mainCategory, retailConnection, transaction);
                    if (result.IsNull())
                    {
                        transaction.Rollback();
                        throw new ResultIsNullException("Cannot Create Main Category");
                    }
                    await subCategoryRepository.Create(new SubCategory
                    {
                        CreateBy = mainCategory.CreateBy,
                        Created = mainCategory.Created,
                        MainCategoryId = result!.Id,
                        NameEn = mainCategory.NameEn,
                        NameTh = mainCategory.NameTh,
                        LastUpdated = mainCategory.LastUpdated,
                        LastUpdatedBy = mainCategory.LastUpdatedBy,
                        Description = mainCategory.Description,
                        Primary = true
                    }, retailConnection, transaction);
                    transaction.Commit();
                    retailConnection.Close();
                    return true;
                });

        public async Task<Either<Exception, int>> CreateMainCategories(IEnumerable<MainCategory> mainCategories) =>
            await InvokeDapperServiceAsync(async retailConnection =>
            {
                retailConnection.Open();
                using var transaction = retailConnection.BeginTransaction();
                var result = await mainCategoryRepository.Creates(mainCategories, retailConnection, transaction);
                if (result.Count.NotEq(mainCategories.Count()))
                {
                    transaction.Rollback();
                    throw new ResultIsNullException("Cannot Create Main Categories");
                }
                await subCategoryRepository.Creates(result.Select(mainCategory => new SubCategory()
                {
                    CreateBy = mainCategory.CreateBy,
                    Created = mainCategory.Created,
                    MainCategoryId = mainCategory.Id,
                    NameEn = mainCategory.NameEn,
                    NameTh = mainCategory.NameTh,
                    LastUpdated = mainCategory.LastUpdated,
                    LastUpdatedBy = mainCategory.LastUpdatedBy,
                    Description = mainCategory.Description,
                    Primary = true
                }), retailConnection, transaction);
                transaction.Commit();
                retailConnection.Close();
                return mainCategories.Count();
            });

        public async Task<Either<Exception, bool>> UpdateMainCategory(MainCategory mainCategory) =>
            await InvokeDapperServiceAsync(async retailConnection =>
            {
                var primarySubCategory = await subCategoryRepository.GetPrimarySubCategory(mainCategory.Id);
                mainCategory.LastUpdated = DateTime.UtcNow;
                retailConnection.Open();
                using var transaction = retailConnection.BeginTransaction();
                await mainCategoryRepository.Update(mainCategory, retailConnection, transaction);
                primarySubCategory!.NameEn = mainCategory.NameEn;
                primarySubCategory!.NameTh = mainCategory.NameTh;
                primarySubCategory!.Description = mainCategory.Description;
                primarySubCategory!.LastUpdated = mainCategory.LastUpdated;
                primarySubCategory!.LastUpdatedBy = mainCategory.LastUpdatedBy;
                await subCategoryRepository.Update(primarySubCategory, retailConnection, transaction);
                transaction.Commit();
                retailConnection.Close();
                return true;
            });

        public async Task<Either<Exception, bool>> DeleteMainCategory(long id) =>
            await InvokeServiceAsync(async () =>
            {
                await subCategoryRepository.DeleteByMainCategory(id);
                return await mainCategoryRepository.Delete(id);
            });

        public async Task<Either<Exception, List<MainCategory>>> ListMainCategories(string? search = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.ListMainCategories(search, page, per));

        public async Task<Either<Exception, MainCategory>> GetMainCategory(long id) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.GetMainCategory(id));

        public async Task<Either<Exception, int>> GetCountMainCategory(string? search = null) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.GetCount(search));

        public async Task<Either<Exception, int>> GetLastPageMainCategory(string? search = null, int per = 20) =>
            await InvokeServiceAsync(async () => await mainCategoryRepository.GetLastPage(search, per));

        public async Task<Either<Exception, bool>> CreateSubCategory(SubCategory subCategory) =>
            await InvokeDapperServiceAsync(async retailConnection => await subCategoryRepository.Create(subCategory, retailConnection));

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

        public async Task<Either<Exception, List<MainCategoryExcelModelValidator>>> ValidateExcelData(IEnumerable<MainCategoryExcelModel> mainCategories) =>
            await InvokeServiceAsync(async () =>
            {
                var nameList = mainCategories.Select(x => x.Name_EN).ToList();
                nameList.If(x => x.HasData(),
                    () => nameList = nameList.Union(mainCategories.Select(x => x.Name_TH)).ToList(),
                    () => nameList = mainCategories.Select(x => x.Name_TH).ToList()
                    );
                var existingCategories = await mainCategoryRepository.ListByNameENOrTH(nameList.ToArray());
                List<MainCategoryExcelModelValidator> returnValues = new List<MainCategoryExcelModelValidator>();
                await mainCategories.HasDataAndForEachAsync(m =>
                {
                    bool nameEmpty = m.Name_EN.IsNullOrEmpty();
                    MainCategoryExcelModelValidator newItem = new();
                    m.Name_TH.IfNull(m.Name_EN);
                    newItem.Entity = m;
                    newItem.Key = m.Name_EN;
                    newItem.IsCorruped = m.Name_EN.IsNullOrEmpty()
                    .Or(existingCategories.Where(x=>x.NameEn.InCaseSensitiveEquals(m.Name_EN) || x.NameTh.InCaseSensitiveEquals(m.Name_TH)).HasData());
                    if(newItem.IsCorruped)
                    {
                        if(nameEmpty)
                        {
                            newItem.Message = "Name_EN is Empty";
                        }
                        else
                        {
                            newItem.Message = "Name_EN or Name_TH is already exist";
                        }
                    }
                    returnValues.Add(newItem);
                });
                return returnValues;
            });
    }
}
