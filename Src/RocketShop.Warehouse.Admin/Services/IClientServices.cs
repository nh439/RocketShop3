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
        Task<Either<Exception, List<Client>>> ListClient(string? search = null, int? page = null, int per = 10);
        Task<Either<Exception, Client?>> GetClient(long clientId);
        Task<Either<Exception, long>> CountClient(string? search = null);
        Task<Either<Exception, int>> GetLastpage(string? search = null, int per = 10);
        Task<Either<Exception, Client>> Create(Client client);
        Task<Either<Exception, bool>> Update(Client client);
        Task<Either<Exception, bool>> Delete(long id);
        Task<Either<Exception, long>> GetUnSafeClient();
        Task<Either<Exception, long>> GetIncompletedClient();
        Task<Either<Exception, long[]>> ListUnSafeClientId();
        Task<Either<Exception, long[]>> ListIncompleteClientId();
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

        public async Task<Either<Exception, long>> CountClient(string? search = null) =>
            await InvokeServiceAsync(async () => await clientRepository.CountClient(search));

        public async Task<Either<Exception, int>> GetLastpage(string? search = null, int per = 10) =>
            await InvokeServiceAsync(async () =>await clientRepository.GetLastpage(search, per));

        public async Task<Either<Exception, Client>> Create(Client client) =>
            await InvokeServiceAsync(async () => await clientRepository.Create(client));

        public async Task<Either<Exception, bool>> Update(Client client)=>
            await InvokeServiceAsync(async() => await clientRepository.Update(client));

        public async Task<Either<Exception, bool>> Delete(long id)=>
            await InvokeServiceAsync(async () => await clientRepository.Delete(id));

        public async Task<Either<Exception, long>> GetUnSafeClient() =>
            await InvokeServiceAsync(async () => await clientRepository.GetUnsafeClient());

        public async Task<Either<Exception,long>> GetIncompletedClient() =>
            await InvokeServiceAsync(async () => await clientRepository.GetIncompleteClient());

        public async Task<Either<Exception, long[]>> ListUnSafeClientId() =>
            await InvokeServiceAsync(async () => await clientRepository.ListUnSafeClientId());

        public async Task<Either<Exception, long[]>> ListIncompleteClientId() =>
            await InvokeServiceAsync(async () => await clientRepository.ListIncompleteClientId());

    }
}
