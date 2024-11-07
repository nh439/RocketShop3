using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Framework.Services;
using RocketShop.Warehouse.Admin.Repository;

namespace RocketShop.Warehouse.Admin.Services
{
    public interface IClientServices
    {
    }
    public class ClientServices(
        ILogger<ClientServices> logger,
        IConfiguration configuration,
        ClientRepository clientRepository) : BaseServices<ClientServices>(
            "Client Service",
            logger,
            new NpgsqlConnection(configuration.GetWarehouseConnectionString()
                )), IClientServices
    {
        public async Task<Either<Exception, List<Client>>> ListClient(string? search = null, int? page = null, int per = 10) =>
            await InvokeServiceAsync(async () => await clientRepository.ListClient(search, page, per));

        public async Task<Either<Exception, Client?>> GetClient(long clientId) =>
            await InvokeServiceAsync(async () => await clientRepository.GetClient(clientId));

        public async Task<Either<Exception, int>> CountClient(string? search = null) =>
            await InvokeServiceAsync(async () => await clientRepository.CountClient(search));

        public async Task<Either<Exception, int>> GetLastpage(string? search = null, int per = 10) =>
            await InvokeServiceAsync(async () =>await clientRepository.GetLastpage(search, per));

        public async Task<Either<Exception, bool>> Create(Client client) =>
            await InvokeServiceAsync(async () => await clientRepository.Create(client));
    }
}
