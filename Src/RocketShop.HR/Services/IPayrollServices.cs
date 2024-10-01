using DocumentFormat.OpenXml.Drawing.Diagrams;
using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IPayrollServices
    {
        Task<Either<Exception, List<UserPayroll>>> ListPayroll(int? page = null, int per = 20);
        Task<Either<Exception, List<UserPayroll>>> ListPayrollByUserId(string userId, int? page = null, int per = 20);
        Task<Either<Exception, List<UserPayroll>>> ListByUserContain(string searchUserKeyword, int? page = null, int per = 20);
        Task<Either<Exception, SlipData>> GetSlipData(string payrollId);
        Task<Either<Exception, bool>> Create(SlipData slipData);
        Task<Either<Exception, int>> BulkCreate(IEnumerable<SlipData> slipData);
        Task<Either<Exception, int>> GetCount(params string[]? UserIdRange);
        Task<Either<Exception, int>> GetLastPage(int per, params string[]? UserIdRange);
    }
    public class PayrollServices(
        ILogger<PayrollServices> logger,
        UserPayrollRepository userPayrollRepository,
        AdditionalPayrollRepository additionalPayrollRepository,
        UserRepository userRepository,
        ProvidentRepository providentRepository,
        IConfiguration configuration) 
        : BaseServices<PayrollServices>("Payroll Service",logger,new NpgsqlConnection(configuration.GetIdentityConnectionString())), IPayrollServices
    {
        public async Task<Either<Exception, List<UserPayroll>>> ListPayroll(int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await userPayrollRepository.GetUserPayrolls(page,per));

        public async Task<Either<Exception, List<UserPayroll>>> ListPayrollByUserId(string userId, int? page = null, int per = 20) => 
            await InvokeServiceAsync(async () => await userPayrollRepository.GetByUserId(userId,page,per));

        public async Task<Either<Exception, List<UserPayroll>>> ListByUserContain(string searchUserKeyword, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () =>
            {
                var relatedUsers = await userRepository.GetUsers(searchUserKeyword);
                return await userPayrollRepository.GetByUserIdRange(relatedUsers.Select(s => s.UserId), page, per);
            });

        public async Task<Either<Exception, SlipData>> GetSlipData(string payrollId) =>
            await InvokeServiceAsync(async () => new SlipData(
                await userPayrollRepository.GetUserPayroll(payrollId),
                await additionalPayrollRepository.GetAdditionalPayrolls(payrollId)
                )
           );

        public async Task<Either<Exception, bool>> Create(SlipData slipData) =>
            await InvokeDapperServiceAsync(async Connection =>
            {
                Connection.Open();
                using var transaction = Connection.BeginTransaction();
                var payroll = slipData.ToUserPayroll()!;
                payroll.TotalAdditionalPay = slipData.HasAdditional ? 
                    slipData.AdditionalPayrolls!.Select(s=>s.Value).Sum() : 0;
                slipData.TotalPayment = slipData.Salary + (payroll.TotalAdditionalPay + payroll.TravelExpenses) - (payroll.ProvidentFund + payroll.SocialSecurites);
                await userPayrollRepository.Create(payroll,Connection,transaction);
                if (slipData.HasAdditional)
                    await additionalPayrollRepository.SetAdditionalPayroll
                    (slipData.PayRollId,
                        slipData.ToAdditionalPayroll()!,
                        Connection,
                        transaction);
                if (slipData.ProvidentFund.NotEq(0))
                {
                   var x = await providentRepository.AddProvidentFundValue(
                        slipData.UserId,
                        slipData.ProvidentFund,
                        slipData.Currency,
                        Connection,
                        transaction);
                    Console.WriteLine(x);
                }
                transaction.Commit();
                Connection.Close();
                return true;
            });

        public async Task<Either<Exception, int>> BulkCreate(IEnumerable<SlipData> slipData) =>
            await InvokeDapperServiceAsync(async con =>
            {
                con.Open();
                using var transaction = con.BeginTransaction();
                var slipInserted = await userPayrollRepository.BulkCreate(slipData.Select(s => s.ToUserPayroll())!,con,transaction);
                foreach( var s in slipData.Where(x=>x.HasAdditional))
                {
                    s.AdditionalPayrolls.HasDataAndForEach(f => f.PayrollId = s.PayRollId);
                        await additionalPayrollRepository.SetAdditionalPayroll(
                            s.PayRollId,
                            s.ToAdditionalPayroll()!,
                            con,
                            transaction);
                    if (s.ProvidentFund.NotEq(0))
                        await providentRepository.AddProvidentFundValue(
                            s.UserId,
                            s.ProvidentFund,
                            s.Currency,
                            con,
                            transaction);
                }
                transaction.Commit();
                con.Close();
                return slipInserted;
            });

        public async Task<Either<Exception, bool>> CancelPayroll(string payrollId) =>
            await InvokeDapperServiceAsync(async con => await userPayrollRepository.CancelPayroll(payrollId, con));

        public async Task<Either<Exception,int>> GetCount(params string[]? UserIdRange) =>
            await InvokeServiceAsync(async () => await  userPayrollRepository.GetCount(UserIdRange));

        public async Task<Either<Exception, int>> GetLastPage(int per, params string[]? UserIdRange) =>
            await InvokeServiceAsync(async () => await userPayrollRepository.GetLastPage(per, UserIdRange));


    }
}
