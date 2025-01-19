using LanguageExt;
using RocketShop.Framework.Extension;

namespace RocketShop.Shared.Model
{
    public class ImportExcelValidator<TKey, TEntity>
    {
        public bool IsCorruped { get; set; }
        public string? Message { get; set; }
        public TKey Key { get; set; }
        public TEntity Entity { get; set; }
    }

    public static class ImportExcelValidatorExtensions
    {
        public static int CorrupedCount<TKey, TEntity>(this IEnumerable<ImportExcelValidator<TKey, TEntity>> importExcelValidator) =>
             importExcelValidator.Count(x => x.IsCorruped);

        public static Option< ImportExcelValidator<TKey, TEntity>> Find<TKey, TEntity>(this IEnumerable<ImportExcelValidator<TKey, TEntity>> importExcelValidator, TKey key) =>
            importExcelValidator.FirstOrDefault(x => x.Key!.Equals(key));

        public static Option<bool> IsCorrupt<TKey, TEntity>(this IEnumerable<ImportExcelValidator<TKey, TEntity>> importExcelValidator, TKey key) =>
            importExcelValidator.Find(key).If(x=> x.IsSome ,
                x => x!.Extract()!.IsCorruped.AsOptional(),
                _ => Option<bool>.None);

        public static IEnumerable<ImportExcelValidator<TKey, TEntity>>? GetCorrupted<TKey, TEntity>(this IEnumerable<ImportExcelValidator<TKey, TEntity>> importExcelValidator) =>
            importExcelValidator.Where(x => x.IsCorruped);

        public static bool HasCorrupted<TKey, TEntity>(this IEnumerable<ImportExcelValidator<TKey, TEntity>> importExcelValidator)
        {
            foreach(var item in importExcelValidator)
                if(item.IsCorruped)
                    return true;
            return false;
        }

        public static IEnumerable<ImportExcelValidator<TKey, TEntity>> RemoveCorrupedItems<TKey, TEntity>(this IEnumerable<ImportExcelValidator<TKey, TEntity>> items)=>
                        items.Where(x => !x.IsCorruped);

    }
}
