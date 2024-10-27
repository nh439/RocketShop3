using Dapper;
using DocumentFormat.OpenXml.Wordprocessing;
using RocketShop.Framework.Extension;
using RocketShop.Warehouse.Admin.Model;
using System.Data;
using TableDescription = RocketShop.Warehouse.Admin.Model.TableDescription;

namespace RocketShop.Warehouse.Admin.Repository
{
    public class TableInformationRepository
    {
        public async Task<IEnumerable<string>> ListTableNames(IDbConnection warehouseConnection, IDbTransaction? transaction = null) =>
            await warehouseConnection.QueryAsync<string>(@"SELECT ""tablename"" FROM pg_catalog.pg_tables 
where ""tablename"" not like 'pg_%' and 
""tablename"" not like 'Authorization_%' and
""tablename"" <> '__EFMigrationsHistory' and
""schemaname"" <> 'information_schema';", transaction:transaction);

         public async Task<IEnumerable<string>> ListViewNames(IDbConnection warehouseConnection, IDbTransaction? transaction = null) =>
            await warehouseConnection.QueryAsync<string>(@"select table_name from INFORMATION_SCHEMA.views WHERE ""table_name"" like 'V_%';",transaction:transaction);

        public async Task<List<TableDescription>> GetTableInformations(string? search, 
            int?page,
            int? per, 
            IDbConnection warehouseConnection, 
            IDbTransaction? transaction = null)
        {
            var tables = await ListTableNames(warehouseConnection, transaction);
            var views = await ListViewNames(warehouseConnection, transaction);
            if (!tables.HasData())
                tables = new List<string>();
            var collections = tables.Select(s => new
            {
                Name = s,
                Table = true
            });
            if (views.HasData())
                collections = collections.Union(views.Select(s =>
               new
               {
                   Name = s,
                   Table = false
               }));
            
            if(!collections.HasData())return new();
            collections = collections.OrderBy(x=> x.Name);
            if (search.HasData())
                collections = collections.Where(x => x.Name.ToLower().Contains(search!));
            if (page.HasValue)
            {
                int pageSize = per ?? 5;
                collections = collections.Skip((page.Value - 1) * pageSize).Take(pageSize);
            }
            List<TableDescription> returnValues = new List<TableDescription>();
            var tableColumns =( await warehouseConnection.QueryAsync(@$"SELECT table_name,column_name
  FROM information_schema.columns
 WHERE  table_name   in ({string.Join(",",collections.Select(se=>$"'{se.Name}'"))})
     ;",transaction: transaction))
     .Select(s=> new
     {
         TableName = s.table_name as string,
         ColumnName = s.column_name as string
     });
            foreach(var table in collections)
            {
                int columns = tableColumns.Where(x=>x.TableName==table.Name).Count();
                long rows = await warehouseConnection.QueryFirstOrDefaultAsync<long>(
                    $"select count(*) from \"{table.Name}\";",
                    transaction: transaction
                    );
                returnValues.Add(new TableDescription(table.Name,table.Table,columns,rows));
            }
            return returnValues;
        }


            
    }
}
