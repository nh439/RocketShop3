using RocketShop.Database;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.Model.Warehouse.Views;
using RocketShop.Framework.Extension;
using RocketShop.Warehouse.Services;

namespace RocketShop.Warehouse.GraphQL
{
    public class GraphQuery(
        IHttpContextAccessor accessor,
        IAddressService addressService,
        ILogger<GraphQuery> logger) : BaseGraphQLServices<GraphQuery>(accessor, logger)
    {
        public async Task<List<Province>?> ListProvince(string? search = null) =>
            await AuthorizeQueryAsync(TableConstraint.Province, async () =>
            {
                var result = await addressService.ListProvince(search);
                if (result.IsLeft) return new();
                return result.GetRight()!;
            });

        public async Task<Province?> GetProvince(int id) =>
            await AuthorizeQueryAsync(TableConstraint.Province, async () =>
        {
            var result = await addressService.GetProvince(id);
            if (result.IsLeft) return new();
            return result.GetRight()!;
        });

        public async Task<List<District>?> ListDistricts(string? search = null, int? provinceId = null) =>
            await AuthorizeQueryAsync(TableConstraint.District, async () =>
        {
            var result = await addressService.ListDistrict(search, provinceId);
            if (result.IsLeft) return new();
            return result.GetRight()!;
        });

        public async Task<District?> GetDistrict(int id) =>
            await AuthorizeQueryAsync(TableConstraint.District, async () =>
        {
            var result = await addressService.GetDistrict(id);
            if (result.IsLeft) return new();
            return result.GetRight()!;
        });

        public async Task<List<SubDistrict>?> ListSubDistricts(string? search = null, int? districtId = null, int? postalCode = null) =>
            await AuthorizeQueryAsync(TableConstraint.SubDistrict, async () =>
        {
            var result = await addressService.ListSubDistrict(search, districtId, postalCode);
            if (result.IsLeft) return new();
            return result.GetRight()!;
        });

        public async Task<SubDistrict?> GetSubDistrict(int id) =>
            await AuthorizeQueryAsync(TableConstraint.SubDistrict, async () =>
        {
            var result = await addressService.GetSubDistrict(id);
            if (result.IsLeft) return new();
            return result.GetRight()!;
        });

        public async Task<List<AddressView>?> ListAddresses(string? search = null, int? page = null, int? pageSize = null) =>
           await AuthorizeQueryAsync(TableConstraint.AddressView, async () =>
       {
           var result = await addressService.ListAddresses(search, page, pageSize);
           if (result.IsLeft) return new();
           return result.GetRight()!;
       });

        public async Task<AddressView?> GetAddress(int id) =>
            await AuthorizeQueryAsync(TableConstraint.AddressView, async () =>
        {
            var result = await addressService.GetAddress(id);
            if (result.IsLeft) return new();
            return result.GetRight()!;
        });
    }



}
