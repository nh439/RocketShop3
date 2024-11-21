using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.Helper;
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
        Task<Either<Exception, bool>> SetAllowedApplication(long clientId,
            List<string>? readAllowedObject,
            List<string>? writeAllowedObject);
        Task<Either<Exception, List<AllowedObject>>> ListAllowedObject(long clientId);
        Task<Either<Exception, bool>> CreateClientSecret(
            long clientId,
            string secretValue,
            string? description = null,
            DateTime? expired = null
            );
        Task<Either<Exception, bool>> DeleteSecret(string secretId);
        Task<Either<Exception, List<ClientSecret>>> ListSecret(long clientId);
    }
    public class ClientServices(
        ILogger<ClientServices> logger,
        IConfiguration configuration,
        ClientRepository clientRepository,
        ClientAllowedObjectRepository clientAllowedObjectRepository,
        ClientSecretRepository clientSecretRepository) : BaseServices<ClientServices>(
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

        public async Task<Either<Exception, bool>> SetAllowedApplication(long clientId,
            List<string>? readAllowedObject,
            List<string>? writeAllowedObject) =>
            await InvokeDapperServiceAsync(async warehouseConnection =>
            {
                List<AllowedObject> allowedObjects = new List<AllowedObject>();
                var allowedList = readAllowedObject?.Union(writeAllowedObject?? new List<string>())
                .Distinct();
                await allowedList.HasDataAndParallelForEachAsync(allowed =>
                {
                    var obj = new AllowedObject
                    {
                        ObjectName = allowed,
                        Read = readAllowedObject.HasData() ? readAllowedObject!.Where(x => x == allowed).HasData() : false,
                        Write = writeAllowedObject.HasData() ? writeAllowedObject!.Where(x => x == allowed).HasData() :false
                    };
                    allowedObjects.Add(obj);
                });
                warehouseConnection.Open();
                using var transaction = warehouseConnection.BeginTransaction();
                var result = await clientAllowedObjectRepository.SetAllowedObject(
                    clientId, 
                    allowedObjects, 
                    warehouseConnection, 
                    transaction);
                transaction.Commit();
                warehouseConnection.Close();
                return result;
            });

        public async Task<Either<Exception, List<AllowedObject>>> ListAllowedObject(long clientId) =>
            await InvokeServiceAsync(async () => await clientAllowedObjectRepository.GetAllowedObject(clientId));

        public async Task<Either<Exception, bool>> CreateClientSecret(
            long clientId,
            string secretValue,
            string? description = null,
            DateTime? expired = null
            ) =>
            await InvokeServiceAsync(async () =>
            {
                var hashed = secretValue.HashPasword(out var salt);
                if(expired.HasValue)
                    expired = DateTime.SpecifyKind(expired.Value, DateTimeKind.Utc);
                var secret = new ClientSecret
                {
                    Client = clientId,
                    Description = description,
                    Expired = expired,
                    Salt = Convert.ToBase64String(salt),
                    SecretValue = hashed
                };
                return await clientSecretRepository.CreateSecret(secret);
            });

        public async Task<Either<Exception, bool>> DeleteSecret(string secretId) =>
            await InvokeServiceAsync(async () => await clientSecretRepository.DeleteSecret(secretId));

        public async Task<Either<Exception, List<ClientSecret>>> ListSecret(long clientId) =>
            await InvokeServiceAsync(async () =>
            {
                var result = await clientSecretRepository.ListSecret(clientId);
                await result.HasDataAndParallelForEachAsync(f => f.SecretValue = "Secret");
                return result;
            });
    }
}
