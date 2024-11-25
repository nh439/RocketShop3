using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Function;
using RocketShop.Framework.Services;
using RocketShop.Shared.Helper;
using RocketShop.Warehouse.Repository;

namespace RocketShop.Warehouse.Services
{
    public interface IAuthenicationService
    {
        Task<Either<Exception, string>> IssueToken(string clientId, string? clientSecret, string application);
    }
    public class AuthenicationService(
        ILogger<AuthenicationService> logger,
        IConfiguration configuration,
        ClientRepository clientRepository,
        ClientSecretRepository clientSecretRepository,
        ClientAllowedObjectRepository clientAllowedObjectRepository,
        TokenRepository tokenRepository) :
        BaseServices<AuthenicationService>("Authenication Service", logger, new NpgsqlConnection(configuration.GetWarehouseConnectionString())), IAuthenicationService
    {
        public async Task<Either<Exception, string>> IssueToken(string clientId, string? clientSecret, string application) =>
            await InvokeDapperServiceAsync(async warehouseConnection =>
            {
                warehouseConnection.Open();
                var clientResult = await clientRepository.GetByClientName(clientId, warehouseConnection);
                if (clientResult.IsNone)
                    throw new ResultIsNullException("Client Not Found.");
                var client = clientResult.Extract()!;
                if(application != client.Application)
                    throw new AccessViolationException("Application Incorrect.");
                bool passSecret = false;
                if (clientSecret.HasMessage() || client.RequireSecret)
                {
                    var secrets = await clientSecretRepository.ListSecret(client.Id, warehouseConnection);
                    await secrets.HasDataAndForEachAsync(s =>
                    {
                        passSecret = passSecret || clientSecret!.VerifyPassword(s.SecretValue, Convert.FromBase64String(s.Salt));
                    });
                }
                else
                    passSecret = true;

                if (!passSecret)
                    throw new AccessViolationException("Secret Invalid.");

                var token = new Token
                {
                    Client = client.Id,
                    IssueDate = DateTime.UtcNow,
                    RemainingAccess = client.MaxinumnAccess,
                    TokenAge = client.TokenExpiration.HasValue ? TimeSpan.FromSeconds(client.TokenExpiration.Value) : null,
                    TokenKey = StringFunction.GenerateRandomString(30)
                };

                using var transaction = warehouseConnection.BeginTransaction();
                var result = await tokenRepository.CreateToken(token, warehouseConnection, transaction);
                transaction.Commit();
                warehouseConnection.Close();
                return result?.TokenKey;
            });


    }
}
