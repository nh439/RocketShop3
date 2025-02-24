﻿using Dapper;
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
            NameTable{
                Name = s,
                Table = true
            }).ToList();
            if (views.HasData())
                collections.AddRange(views.Select(s =>
                new
                NameTable
                {
                    Name = s,
                    Table = false
                }).ToList());
            
            if(!collections.HasData())return new();
            collections = collections.OrderBy(x=> x.Name).ToList();
            if (search.HasData())
                collections = collections.Where(x => x.Name.ToLower().Contains(search!.ToLower())).ToList();
            if (page.HasValue)
            {
                int pageSize = per ?? 5;
                collections = collections.Skip((page.Value - 1) * pageSize).Take(pageSize).ToList();
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
                int columns = tableColumns.Count(x=>x.TableName==table.Name);
                long rows = await warehouseConnection.QueryFirstOrDefaultAsync<long>(
                    $"select count(*) from \"{table.Name}\";",
                    transaction: transaction
                    );
                returnValues.Add(new TableDescription(table.Name,table.Table,columns,rows));
            }
            return returnValues;
        }

        public async Task<int> CountCollections(string? search,IDbConnection warehouseConnection, IDbTransaction? transaction = null) =>
            search.HasMessage() ?
            await warehouseConnection.QueryFirstOrDefaultAsync<int>(@"select
(SELECT count(*)::int FROM pg_catalog.pg_tables 
where ""tablename"" not like 'pg_%' and 
""tablename"" not like 'Authorization_%' and
""tablename"" <> '__EFMigrationsHistory' and
lower(""tablename"") like @search and
""schemaname"" <> 'information_schema')+
(select count(*)::int from INFORMATION_SCHEMA.views WHERE ""table_name"" like 'V_%' and lower(""table_name"") like @search);", new {search = $"%{search!.ToLower()}%"}, transaction: transaction)
            :
            await warehouseConnection.QueryFirstOrDefaultAsync<int>(@"select
(SELECT count(*)::int FROM pg_catalog.pg_tables 
where ""tablename"" not like 'pg_%' and 
""tablename"" not like 'Authorization_%' and
""tablename"" <> '__EFMigrationsHistory' and
""schemaname"" <> 'information_schema')+
(select count(*)::int from INFORMATION_SCHEMA.views WHERE ""table_name"" like 'V_%');", transaction: transaction);

        sealed class NameTable
        {
            public string Name { get; set; }
            public bool Table { get; set; }
        }
    }
}
