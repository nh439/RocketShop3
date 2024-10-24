using DocumentFormat.OpenXml.Drawing.Diagrams;
using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.Model.Warehouse.Views;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Warehouse.Repository;

namespace RocketShop.Warehouse.Services
{
    public interface IAddressService
    {
        Task<Either<Exception, List<Province>>> ListProvince(string? search = null);
        Task<Either<Exception, Province?>> GetProvince(int id);
        Task<Either<Exception, List<District>>> ListDistrict(string? search = null, int? provinceId = null);
        Task<Either<Exception, District?>> GetDistrict(int id);
        Task<Either<Exception, List<SubDistrict>>> ListSubDistrict(string? search = null, int? districtId = null, int? postalCode = null);
        Task<Either<Exception, SubDistrict?>> GetSubDistrict(int id);
        Task<Either<Exception, List<AddressView>>> ListAddresses(string? search = null, int? page = null, int? pageSize = null);
        Task<Either<Exception, AddressView?>> GetAddress(int id);
    }
    public class AddressServices(ILogger<AddressServices> logger,
        IConfiguration configuration,
        ProvinceRepository provinceRepository,
        DistrictRepository districtRepository,
        SubDistrictRepository subDistrictRepository,
        AddressViewRepository addressViewRepository) : 
        BaseServices<AddressServices>("Address Service",logger),IAddressService
    {
        public async Task<Either<Exception, List<Province>>> ListProvince(string? search = null) =>
            await InvokeServiceAsync(async () => {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return await provinceRepository.ListProvince(search, connection);
                });

        public async Task<Either<Exception, Province?>> GetProvince(int id) =>
            await InvokeServiceAsync(async () =>
            {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return (await provinceRepository.GetByCode(id, connection)).Extract();
            });

         public async Task<Either<Exception, List<District>>> ListDistrict(string? search = null,int? provinceId = null) =>
            await InvokeServiceAsync(async () => {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return await districtRepository.ListDistrict(search,provinceId, connection);
                });

        public async Task<Either<Exception, District?>> GetDistrict(int id) =>
            await InvokeServiceAsync(async () =>
            {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return (await districtRepository.GetDistrict(id, connection)).Extract();
            });

         public async Task<Either<Exception, List<SubDistrict>>> ListSubDistrict(string? search = null,int? districtId = null,int? postalCode = null) =>
            await InvokeServiceAsync(async () => {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return await subDistrictRepository.ListSubDistricts(search,districtId,postalCode, connection);
                });

        public async Task<Either<Exception, SubDistrict?>> GetSubDistrict(int id) =>
            await InvokeServiceAsync(async () =>
            {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return (await subDistrictRepository.GetSubDistrict(id, connection)).Extract();
            });

        public async Task<Either<Exception, List<AddressView>>> ListAddresses(string? search = null,int? page = null,int? pageSize = null) =>
            await InvokeServiceAsync(async () => {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return await addressViewRepository.ListAddresses(search,page,pageSize, connection);
                });

        public async Task<Either<Exception, AddressView?>> GetAddress(int id) =>
            await InvokeServiceAsync(async () =>
            {
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                return (await addressViewRepository.GetAddress(id, connection)).Extract();
            });
    }
}
