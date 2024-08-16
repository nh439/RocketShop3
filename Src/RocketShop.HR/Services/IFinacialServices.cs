using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IFinacialServices
    {
    }
    public class FinacialServices(
        Serilog.ILogger logger,
        UserFinacialRepository userFinacialRepository,
        ProvidentRepository providentRepository,
        UserAdditionalExpenseRepository userAdditionalExpenseRepository,
        IConfiguration configuration
        ) : BaseServices("Finacial Service",logger,new NpgsqlConnection(configuration.GetIdentityConnectionString())), IFinacialServices
    {

    }
}
