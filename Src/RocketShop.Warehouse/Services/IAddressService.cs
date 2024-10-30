using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.Model.Warehouse.Views;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.SharedService.Singletion;
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
        AddressViewRepository addressViewRepository,
        IMemoryStorageServices memoryStorageServices) :
        BaseServices<AddressServices>("Address Service", logger), IAddressService
    {
        public async Task<Either<Exception, List<Province>>> ListProvince(string? search = null) =>
            await InvokeServiceAsync(async () =>
            {
                var key = $"ListProvince_{search}";
                if (memoryStorageServices.Exists(key!))
                {
                    var cache = memoryStorageServices.GetData<List<Province>>(key!);
                    if (cache.IsSome) return cache.Extract();
                }
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                var returnObj = await provinceRepository.ListProvince(search, connection);
                memoryStorageServices.AddData(key, returnObj, TimeSpan.FromMinutes(5));
                return returnObj;
            });

        public async Task<Either<Exception, Province?>> GetProvince(int id) =>
            await InvokeServiceAsync(async () =>
            {
                string key = $"GetProvince_{id}";
                if (memoryStorageServices.Exists(key!))
                {
                    var cache = memoryStorageServices.GetData<Province>(key!);
                    if (cache.IsSome) return cache.Extract();
                }
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                var returnObj = (await provinceRepository.GetByCode(id, connection)).Extract();
                if (returnObj.IsNull()) return null;
                memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
                return returnObj;
            });

        public async Task<Either<Exception, List<District>>> ListDistrict(string? search = null, int? provinceId = null) =>
       await InvokeServiceAsync(async () =>
       {
           string key = $"ListDistrict_{search}_{provinceId}";
           if (memoryStorageServices.Exists(key!))
           {
               var cache = memoryStorageServices.GetData<List<District>>(key!);
               if (cache.IsSome) return cache.Extract();
           }
           using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
           var returnObj = await districtRepository.ListDistrict(search, provinceId, connection);
           memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
           return returnObj;
       });

        public async Task<Either<Exception, District?>> GetDistrict(int id) =>
            await InvokeServiceAsync(async () =>
            {
                string key = $"GetDistrict_{id}";
                if (memoryStorageServices.Exists(key!))
                {
                    var cache = memoryStorageServices.GetData<District>(key!);
                    if (cache.IsSome) return cache.Extract();
                }
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                var returnObj = (await districtRepository.GetDistrict(id, connection)).Extract();
                memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
                return returnObj;
            });

        public async Task<Either<Exception, List<SubDistrict>>> ListSubDistrict(string? search = null, int? districtId = null, int? postalCode = null) =>
           await InvokeServiceAsync(async () =>
           {
               string key = $"ListSubDistrict_{search}_{districtId}_{postalCode}";
               if (memoryStorageServices.Exists(key!))
               {
                   var cache = memoryStorageServices.GetData<List<SubDistrict>>(key!);
                   if (cache.IsSome) return cache.Extract();
               }
               using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
               var returnObj = await subDistrictRepository.ListSubDistricts(search, districtId, postalCode, connection);
               memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
               return returnObj;
           });

        public async Task<Either<Exception, SubDistrict?>> GetSubDistrict(int id) =>
            await InvokeServiceAsync(async () =>
            {
                string key = $"GetSubDistrict_{id}";
                if (memoryStorageServices.Exists(key!))
                {
                    var cache = memoryStorageServices.GetData<SubDistrict>(key!);
                    if (cache.IsSome) return cache.Extract();
                }
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                var returnObj = (await subDistrictRepository.GetSubDistrict(id, connection)).Extract();
                memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
                return returnObj;
            });

        public async Task<Either<Exception, List<AddressView>>> ListAddresses(string? search = null, int? page = null, int? pageSize = null) =>
            await InvokeServiceAsync(async () =>
            {
                string key = $"ListAddresses_{search}_{page}_{pageSize}";
                if (memoryStorageServices.Exists(key!))
                {
                    var cache = memoryStorageServices.GetData<List<AddressView>>(key!);
                    if (cache.IsSome) return cache.Extract();
                }
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                var returnObj = await addressViewRepository.ListAddresses(search, page, pageSize, connection);
                memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
                return returnObj;
            });

        public async Task<Either<Exception, AddressView?>> GetAddress(int id) =>
            await InvokeServiceAsync(async () =>
            {
                string key = $"GetAddress_{id}";
                if (memoryStorageServices.Exists(key!))
                {
                    var cache = memoryStorageServices.GetData<AddressView>(key!);
                    if (cache.IsSome) return cache.Extract();
                }
                using var connection = new NpgsqlConnection(configuration.GetWarehouseConnectionString());
                var returnObj = (await addressViewRepository.GetAddress(id, connection)).Extract();
                memoryStorageServices.AddData(key, returnObj!, TimeSpan.FromMinutes(5));
                return returnObj;
            });
    }
}
