namespace RocketShop.Warehouse.Admin.Model
{
    public sealed class FlexibleDataReport
    {
        public List<Dictionary<string,object>>? Data { get; set; }
        public string[] Columns { get; set; }
        public long TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public string GetColumns() => string.Join(";", Columns);
    }
}
