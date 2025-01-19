using RocketShop.Database.Model.Retail;
using RocketShop.Shared.Model;

namespace RocketShop.Retail.Model
{
    public sealed record MainCategoryExcelModel(
        string Name_TH,
        string Name_EN,
        string? Description = null
        );

    public static class MainCategoryExcelModelExtensions
    {
        public static MainCategoryExcelModel ToExcelModel(this MainCategory mainCategory) =>
            new MainCategoryExcelModel(
                mainCategory.NameTh ?? string.Empty,
                mainCategory.NameEn,
                mainCategory.Description
                );

        public static IEnumerable<MainCategoryExcelModel> ToExcelModels(this IEnumerable<MainCategory> mainCategories) =>
            mainCategories.Select(x => x.ToExcelModel());
        public static List<MainCategory> ToEntityModel(this IEnumerable<MainCategoryExcelModel> mainCategories,
            string? createBy = null) =>
            mainCategories.Select(x => new MainCategory
            {
                NameTh = x.Name_TH,
                NameEn = x.Name_EN,
                Description = x.Description,
                Created = DateTime.UtcNow,
                LastUpdated = DateTime.UtcNow,
                CreateBy = createBy ?? string.Empty,
                LastUpdatedBy = createBy ?? string.Empty
            }).ToList();

        public static List<MainCategory> ToEntityModel(this IEnumerable<MainCategoryExcelModelValidator> mainCategories)
        {
            var result = new List<MainCategory>();
            foreach (var item in mainCategories)
            {
                if (item.IsCorruped)
                    continue;
                result.Add(new MainCategory
                {
                    NameTh = item.Entity.Name_TH,
                    NameEn = item.Entity.Name_EN,
                    Description = item.Entity.Description,
                    Created = DateTime.UtcNow,
                    LastUpdated = DateTime.UtcNow,
                    CreateBy = string.Empty,
                    LastUpdatedBy = string.Empty
                });
            }
            return result;



        }
    }
    public class MainCategoryExcelModelValidator : ImportExcelValidator<string, MainCategoryExcelModel>
    {

    }
}

