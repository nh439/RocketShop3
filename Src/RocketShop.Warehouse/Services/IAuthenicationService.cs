using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Function;
using RocketShop.Framework.Services;
using RocketShop.Shared.Helper;
using RocketShop.Shared.Model;
using RocketShop.Warehouse.Repository;

namespace RocketShop.Warehouse.Services
{
    public interface IAuthenicationService
    {
        Task<Either<Exception, string>> IssueToken(string clientId, string? clientSecret, string application);
        Task<Either<Exception, WarehouseTokenIntrospectionResult>> Introspection(string tokenKey);
        Task<Either<Exception, bool>> UsableCheck(string token);
    }
    public class AuthenicationService(
        ILogger<AuthenicationService> logger,
        IConfiguration configuration,
        ClientRepository clientRepository,
        ClientSecretRepository clientSecretRepository,
        ClientAllowedObjectRepository clientAllowedObjectRepository,
        TokenRepository tokenRepository,
        ClientHistoryRepository clientHistoryRepository,
        IHttpContextAccessor accessor) :
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
                if (application != client.Application)
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

        public async Task<Either<Exception, WarehouseTokenIntrospectionResult>> Introspection(string tokenKey) =>
            await InvokeDapperServiceAsync(async warehouseConnection =>
            {
                warehouseConnection.Open();
                var result = await tokenRepository.Introspection(tokenKey, warehouseConnection);
                if (result.IsNone)
                    throw new NullReferenceException("Token Invalid");
                var token = result.Extract()!;
                var clientId = await clientRepository.GetClientIdById(token.Client, warehouseConnection);
                bool usable = (token.RemainingAccess.HasValue ? token.RemainingAccess > 0 : true) &&
                    (token.TokenAge.HasValue ? DateTime.UtcNow <= token.IssueDate.Add(token.TokenAge.Value!) : true);

                return new WarehouseTokenIntrospectionResult
                {
                    TokenKey = tokenKey,
                    ClientId = clientId.Extract()!,
                    ExpiredDate = token.TokenAge.IsNotNull() ? token.IssueDate.Add(token.TokenAge!.Value) : null,
                    IssueDate = token.IssueDate,
                    RemainingAccess = token.RemainingAccess,
                    TokenAge = token.TokenAge,
                    Usable = usable,
                    AccessLimitReached = (!(token.RemainingAccess.HasValue ? token.RemainingAccess > 0 : true)),
                    TokenExpired = (!(token.TokenAge.HasValue ? DateTime.UtcNow <= token.IssueDate.Add(token.TokenAge.Value!) : true))
                };
            });

        public async Task<Either<Exception, bool>> UsableCheck(string token) =>
            await InvokeDapperServiceAsync(async warehouseConnection => await tokenRepository.UsableCheck(token, warehouseConnection));

        public async Task<Either<Exception, List<AllowedObject>>> UseToken(string token) =>
            await InvokeDapperServiceAsync(async warehouseConnection =>
            {
                warehouseConnection.Open();
                var usable = await tokenRepository.UsableCheck(token,warehouseConnection);
                if (!usable)
                    throw new AccessViolationException("Token Useless.");
                var tokenInfoResult = await tokenRepository.Introspection(token, warehouseConnection);
                if(tokenInfoResult.IsNone)
                    throw new AccessViolationException("Token Invalid.");
                var tokenInfo = tokenInfoResult.Extract()!;
                using (var transaction = warehouseConnection.BeginTransaction())
                {
                    await tokenRepository.UseToken(token, warehouseConnection, transaction);
                    await clientHistoryRepository.CreateHistory(new ClientHistory
                    {
                        ClientId = tokenInfo.Client,
                        Date = DateTime.UtcNow,
                        Key = token,
                        IpAddress = accessor.GetClientIpAddress()
                    }, warehouseConnection, transaction);
                    transaction.Commit();
                }
                return await clientAllowedObjectRepository.ListAllowedObject(tokenInfo.Client,warehouseConnection);         
            },
                retries: 3,
                intervalSecond: 3
    );

    }
}
