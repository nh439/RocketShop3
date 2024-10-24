using LanguageExt;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse.Views;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class AddressViewRepository
    {
        readonly string tableName = TableConstraint.AddressView;

        public async Task< List<AddressView>> ListAddresses(string? search,
            int? page,
            int? pageSize,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction =null)
        {
            var query = warehouseConnection.CreateQueryStore(tableName, true);
            if (search.HasMessage())
            {
                var cols = new[]
                {
                    nameof(AddressView.DistrictNameEN),
                    nameof(AddressView.DistrictNameTH),
                    nameof(AddressView.SubDistrictNameEN),
                    nameof(AddressView.SubDistrictNameTH),
                    nameof(AddressView.ProvinceNameTH),
                    nameof(AddressView.ProvinceNameEN),
                };
                cols.HasDataAndForEach(f =>
                {
                    query = query.OrWhere(f, SqlOperator.Contains, search!);
                });
            }
            if(page.HasValue)
                query = query.UsePaging(page.Value,pageSize ?? 100);
            return await query.ToListAsync<AddressView>(transaction);
        }

        public async Task<Option<AddressView>> GetAddress(int id,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName, true)
            .Where(nameof(AddressView.Id), id)
            .FetchOneAsync<AddressView>(transaction);
    }
}
