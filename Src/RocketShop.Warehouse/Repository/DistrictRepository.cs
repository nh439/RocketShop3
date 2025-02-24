﻿using LanguageExt;
using RocketShop.Database;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.Warehouse.Repository
{
    public class DistrictRepository(IConfiguration configuration)
    {
        readonly string tableName = TableConstraint.District;
        readonly bool enabledSqlLogging = configuration.GetSection("EnabledSqlLogging").Get<bool>();
        public async Task< List<District>> ListDistrict(
            string? searchName,
            int? provinceId,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null)
        {
            var query = warehouseConnection.CreateQueryStore(tableName, enabledSqlLogging);
            if (searchName.HasMessage())
                query = query.Where(x => x.Where(nameof(District.NameTH), SqlOperator.Contains, searchName!).OrWhere(nameof(District.NameEN), SqlOperator.Contains, searchName!));
            if (provinceId.HasValue)
                query = query.Where(nameof(District.ProvinceId), provinceId.Value);
            return await query.ToListAsync<District>(transaction);   
        }

        public async Task<Option<District>> GetDistrict(int id,
            IDbConnection warehouseConnection,
            IDbTransaction? transaction = null) =>
            await warehouseConnection.CreateQueryStore(tableName,enabledSqlLogging)
            .Where(nameof(District.Id),id)
            .FetchOneAsync<District>(transaction);
    }
}
