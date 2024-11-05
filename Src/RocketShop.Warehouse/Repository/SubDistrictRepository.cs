using LanguageExt;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class SubDistrictRepository(IConfiguration configuration)
    {
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();
        readonly string tableName = TableConstraint.SubDistrict;
        public async Task<List<SubDistrict>> ListSubDistricts(string? search,
            int? districtId,
            int? postalCode,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null)
        {
            var query = warehouseConnection.CreateQueryStore(tableName,enabledSqlLogging);
            if (search.HasMessage())
                query = query.Where(x =>
                x.Where(nameof(SubDistrict.NameTH), SqlOperator.Contains, search!)
                .OrWhere(nameof(SubDistrict.NameEN), SqlOperator.Contains, search!)
                );
            if (postalCode.HasValue)
                query = query.Where(nameof(SubDistrict.PostalCode), postalCode.Value);
            if(districtId.HasValue)
                query = query.Where(nameof(SubDistrict.DistrictId),districtId.Value);
            return await query.ToListAsync<SubDistrict>(transaction);
        }

        public async Task<Option<SubDistrict>> GetSubDistrict(int id,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, enabledSqlLogging)
            .Where(nameof(SubDistrict.Id), id)
            .FetchOneAsync<SubDistrict>();
    }
}
