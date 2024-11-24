using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Framework.Services;
using RocketShop.Warehouse.Repository;

namespace RocketShop.Warehouse.Services
{
    public interface IAuthenicationService
    {
    }
    public class AuthenicationService(
        ILogger<AuthenicationService> logger,
        IConfiguration configuration,
        ClientRepository clientRepository,
        ClientSecretRepository clientSecretRepository,
        ClientAllowedObjectRepository clientAllowedObjectRepository,
        TokenRepository tokenRepository) : 
        BaseServices<AuthenicationService>("Authenication Service",logger,new NpgsqlConnection(configuration.GetWarehouseConnectionString())), IAuthenicationService
    {

    }
}
