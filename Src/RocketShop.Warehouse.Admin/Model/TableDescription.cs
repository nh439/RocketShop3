namespace RocketShop.Warehouse.Admin.Model
{
    public sealed record TableDescription(
        string TableName,
        bool Table,
        int Columns,
        long Rows
        );
    
}
