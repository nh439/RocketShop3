using RocketShop.Framework.Extension;

namespace RocketShop.Warehouse.Admin.Model
{
    public sealed class FlexibleDataReport
    {
        public List<Dictionary<string, object>>? Data { get; set; }
        public string[]? Columns { get; set; }
        public long TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string GetColumns() => Columns.HasData() ? string.Join(";", Columns!) : string.Empty;
        public long GetDataCount() => Data.HasData() ? TotalCount : 0;
        public long GetCellCount()  =>Columns.HasData() ?  GetDataCount() * Columns!.LongLength : 0;
    }
}
