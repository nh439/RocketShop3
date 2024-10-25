using LanguageExt;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class ProvinceRepository(IConfiguration configuration)
    {
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();
        readonly string tableName = TableConstraint.Province;
        public async Task<List<Province>> ListProvince(
            string? searchName, 
            IDbConnection warehouseConnection,
            IDbTransaction? transaction=null)
        {
            var query = warehouseConnection.CreateQueryStore(tableName, enabledSqlLogging);
            if (searchName.HasMessage())
                query = query.Where(nameof(Province.NameTH), SqlOperator.Contains, searchName!)
                    .OrWhere(nameof(Province.NameEN), SqlOperator.Contains, searchName!);
            return await query.ToListAsync<Province>(transaction);
            
        }
        public async Task<Option<Province>> GetByCode(int provinceId,
             IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, enabledSqlLogging)
            .Where(nameof(Province.Id), provinceId)
            .FetchOneAsync<Province>(transaction);
          
       
    }
}
