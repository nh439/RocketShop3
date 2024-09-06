using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IPayrollServices
    {
    }
    public class PayrollServices(
        ILogger<PayrollServices> logger,
        UserPayrollRepository userPayrollRepository,
        AdditionalPayrollRepository additionalPayrollRepository,
        IConfiguration configuration) 
        : BaseServices<PayrollServices>("Payroll Service",logger,new NpgsqlConnection(configuration.GetIdentityConnectionString())), IPayrollServices
    {
        public async Task<Either<Exception, List<UserPayroll>>> ListPayrollAsync(int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await userPayrollRepository.GetUserPayrolls(page,per));
    }
}
