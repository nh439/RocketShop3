using RocketShop.Database.Model.Retail;

namespace RocketShop.Retail.Model
{
    public sealed record SubCategoryExcelModel(
        string nameTH,
        string nameEN,
        string mainCategoryName,
        string? description = null
        );
    public static class SubCategoryExcelModelExtensions
    {
        public static SubCategoryExcelModel ToExcelModel(this SubCategory subCategory,string mainCategoryName) =>
            new SubCategoryExcelModel(
                subCategory.NameTh ?? string.Empty,
                subCategory.NameEn,
                mainCategoryName,
                subCategory.Description
                );

        public static IEnumerable<SubCategoryExcelModel> ToExcelModels(this IEnumerable<SubCategory> subCategories,IEnumerable<MainCategory> mainCategories) =>
            subCategories.Select(x => x.ToExcelModel(mainCategories.FirstOrDefault(y=>x.Id==y.Id)!.ToString()));

        public static List<SubCategory> ToEntityModel(this IEnumerable<SubCategoryExcelModel> subCategories,
            IEnumerable<MainCategory> mainCategories,
            string? createBy = null) =>
            subCategories.Select(x => new SubCategory
            {
                NameTh = x.nameTH,
                NameEn = x.nameEN,
                Description = x.description,
                MainCategoryId = mainCategories.FirstOrDefault(y => y.ToString() == x.mainCategoryName)!.Id,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                CreateBy = createBy ?? string.Empty,
                LastUpdatedBy = createBy ?? string.Empty
            }).ToList();



    }
