using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Enum;
using RocketShop.HR.LocalExtension;
using RocketShop.HR.LocalModel;
using RocketShop.HR.Repository;
using RocketShop.Shared.Model.ExcelModel;
using System.Linq;
using System.Linq.Expressions;
using System.Security;

namespace RocketShop.HR.Services
{
    public interface IFinacialServices
    {
        Task<Either<Exception, List<UserFinancialView>>> ListFinacialData(string? searchName = null, int? page = null, int per = 20);
        Task<Either<Exception, Option<UserFinacialData>>> GetFinacialData(string userId, bool includedAdditionalExpense = true);
        Task<Either<Exception, bool>> CreateFinacialData(UserFinacialData data, UserProvidentFund? userProvidentFund = null);
        Task<Either<Exception, bool>> UpdateFinacialData(UserFinancal financal);
        Task<Either<Exception, bool>> SetAdditionalExpense(string userId, IEnumerable<UserAddiontialExpense> expenses);
        Task<Either<Exception, bool>> AddProvidentFund(UserProvidentFund fund);
        Task<Either<Exception, bool>> UpdateProvidentFund(UserProvidentFund fund);
        Task<Either<Exception, int>> ListFinacialViewCount(string? searchName = null);
        Task<Either<Exception, int>> ListFinacialViewLastpage(string? searchName = null, int per = 20);
        Task<Either<Exception, List<UserView>>> ListNoFinacialDataUsers(string? searchKeyword = null, int? take = null);
        Task<Either<Exception, bool>> UpdateProvidentFundRate(string userId, decimal newProvidentFundRate);
        Task<Either<Exception, List<InputOutputUserFinacialData>>> GetOutputUserFinancialData(string? searchKeyword);
        Task<Either<Exception, List<InputFinacialDataVerify>>> VerfyInputFinacialData(List<InputOutputUserFinacialData> inputData);
        Task<Either<Exception, int>> UpSertFinancialData(IEnumerable<InputFinacialDataVerify> inputs);
    }
    public class FinacialServices(
        ILogger<FinacialServices> logger,
        UserFinacialRepository userFinacialRepository,
        ProvidentRepository providentRepository,
        UserAdditionalExpenseRepository userAdditionalExpenseRepository,
        UserRepository userRepository,
        IConfiguration configuration
        ) : BaseServices<FinacialServices>("Finacial Service", logger, new NpgsqlConnection(configuration.GetIdentityConnectionString())), IFinacialServices
    {
        public async Task<Either<Exception, List<UserFinancialView>>> ListFinacialData(string? searchName = null, int? page = null, int per = 20) =>
            await InvokeServiceAsync(async () => await userFinacialRepository.ListFinancialData(searchName, page, per));

        public async Task<Either<Exception, Option<UserFinacialData>>> GetFinacialData(string userId, bool includedAdditionalExpense = true) =>
            await InvokeServiceAsync(async () =>
            {
                var finacial = await userFinacialRepository.GetUserFinancal(userId);
                var providentFund = await providentRepository.GetUserProvidentFund(userId);
                if (includedAdditionalExpense)
                {
                    var expenses = await userAdditionalExpenseRepository.ListAdditionalExpenseByUserId(userId);
                    return new UserFinacialData(finacial!, expenses, providentFund).AsOptional();
                }
                return new UserFinacialData(finacial!, null, providentFund).AsOptional();
            });

        public async Task<Either<Exception, bool>> CreateFinacialData(UserFinacialData data, UserProvidentFund? userProvidentFund = null) =>
            await InvokeDapperServiceAsync(async con =>
            {
                con!.Open();
                using var transaction = con.BeginTransaction();
                var createResult = await userFinacialRepository.CreateFinacialData(data.GetUserFinancal, con, transaction);
                if (data.AddiontialExpenses.HasData())
                    createResult = createResult && await userAdditionalExpenseRepository.
                    Creates(data.AddiontialExpenses!, con, transaction)
                    .EqAsync(data.AddiontialExpenses!.Count);
                if (userProvidentFund.IsNotNull())
                    createResult = createResult && await providentRepository.Create(userProvidentFund!, con, transaction);
                if (!createResult)
                {
                    transaction.Rollback();
                    con.Close();
                    throw new OperationCanceledException("Error While Create Finacial Data >> Operation Cancelled");
                }
                transaction.Commit();
                con.Close();
                return true;
            });

        public async Task<Either<Exception, bool>> UpdateFinacialData(UserFinancal financal) =>
            await InvokeDapperServiceAsync(async con => await userFinacialRepository.UpdateFinacialData(financal, con));

        public async Task<Either<Exception, bool>> SetAdditionalExpense(string userId, IEnumerable<UserAddiontialExpense> expenses) =>
            await InvokeDapperServiceAsync(async con =>
            {
                expenses.HasDataAndForEach(f =>
                {
                    f.UserId = userId;
                });
                var oldAdditional = await userAdditionalExpenseRepository.ListAdditionalExpenseByUserId(userId);
                con.Open();
                var data = await userFinacialRepository.GetUserFinancal(userId);
                if (data.IsNull())
                    throw new NullReferenceException("User Not Found");
                using var transaction = con.BeginTransaction();
                if (oldAdditional.HasData())
                    await userAdditionalExpenseRepository.Deletes(oldAdditional.Select(x => x.Id).ToArray(), con, transaction);
                if (expenses.HasData())
                    await userAdditionalExpenseRepository.Creates(expenses, con, transaction);
                data!.TotalAddiontialExpense = expenses.Where(x => x.PreiodType == AddiontialExpenseEnum.Monthly).Sum(s => s.Balance);
                var fixPayment = data.SocialSecurites + data.ProvidentFund;
                data.TotalPayment = data.Salary - fixPayment + (data.TravelExpenses + data.TotalAddiontialExpense);
                await userFinacialRepository.UpdateFinacialData(data, con, transaction);
                transaction.Commit();
                con.Close();
                return true;
            });

        public async Task<Either<Exception, bool>> AddProvidentFund(UserProvidentFund fund) =>
            await InvokeDapperServiceAsync(async con => await providentRepository.Create(fund, con));

        public async Task<Either<Exception, bool>> UpdateProvidentFund(UserProvidentFund fund) =>
            await InvokeDapperServiceAsync(async con => await providentRepository.Update(fund, con));

        public async Task<Either<Exception, int>> ListFinacialViewCount(string? searchName = null) =>
            await InvokeServiceAsync(async () => await userFinacialRepository.ListFinacialCount(searchName));

        public async Task<Either<Exception, int>> ListFinacialViewLastpage(string? searchName = null, int per = 20) =>
           await InvokeServiceAsync(async () => await userFinacialRepository.ListFinacialLastpage(searchName, per));

        public async Task<Either<Exception, List<UserView>>> ListNoFinacialDataUsers(string? searchKeyword = null, int? take = null) =>
            await InvokeServiceAsync(async () =>
            {
                var userId = await userFinacialRepository.ListUsersWhenHasFinacialData();
                return await userRepository.ListUserNOTIn(userId, searchKeyword, true, take);
            });
        public async Task<Either<Exception, bool>> UpdateProvidentFundRate(string userId, decimal newProvidentFundRate) =>
            await InvokeDapperServiceAsync(async con =>
            {
                var item = await userFinacialRepository.GetUserFinancal(userId);
                if (item.IsNull())
                    return false;
                var fixPayment = item!.SocialSecurites + newProvidentFundRate;
                item.TotalPayment = item.Salary - fixPayment + (item.TravelExpenses + item.TotalAddiontialExpense);
                return await userFinacialRepository.UpdateProvidentFundRate(userId, newProvidentFundRate, item.TotalPayment, con);
            });

        public async Task<Either<Exception, List<InputOutputUserFinacialData>>> GetOutputUserFinancialData(string? searchKeyword) =>
            await userFinacialRepository.ListFinancialData(searchKeyword)
            .Map(financialData => financialData.Select(s =>
            new InputOutputUserFinacialData
            {
                EmployeeCode = s.EmployeeCode,
                BankName = s.BankName,
                AccountNo = s.AccountNo,
                Salary = s.Salary,
                SocialSecurites = s.SocialSecurites,
                TravelExpense = s.TravelExpenses,
                ProvidentFundPerMonth = s.ProvidentFundPerMonth
            }
            ).ToList());

        public async Task<Either<Exception, List<InputFinacialDataVerify>>> VerfyInputFinacialData(List<InputOutputUserFinacialData> inputData) =>
            await InvokeServiceAsync(async () =>
            {
                var users = await userRepository.ListUserViewByEmployeeCode(inputData.Select(s => s.EmployeeCode).ToArray());
                if (!users.HasData())
                    return inputData.Select(s => new InputFinacialDataVerify(
                        s.EmployeeCode,
                        s.BankName,
                        s.AccountNo,
                        s.Salary,
                        s.SocialSecurites,
                        s.TravelExpense,
                        s.ProvidentFundPerMonth,
                        IsCorrupt: true
                        )).ToList();
                return inputData.Select(s => new InputFinacialDataVerify(
                       s.EmployeeCode,
                       s.BankName,
                       s.AccountNo,
                       s.Salary,
                       s.SocialSecurites,
                       s.TravelExpense,
                       s.ProvidentFundPerMonth,
                       users.FirstOrDefault(x => x.EmployeeCode == s.EmployeeCode)?.UserId,
                       users.FirstOrDefault(x => x.EmployeeCode == s.EmployeeCode).IsNull() ?
                       true : users.Where(x => x.EmployeeCode == s.EmployeeCode).Count().Ge(1)
                       )).ToList();

            });

        public async Task<Either<Exception, int>> UpSertFinancialData(IEnumerable<InputFinacialDataVerify> inputs) =>
            await InvokeDapperServiceAsync(async con =>
            {
                int returnValue = 0;
                if (!inputs.VerifyFinancialData())
                    throw new VerificationException("Cannot UpSert,Because Has Corruped Data");
                var userIds = inputs.Select(s => s.UserId).Distinct();
                var existsData = userIds.HasData() ?  await userFinacialRepository.ListFinancialDataByUserIn(userIds!) : new List<UserFinancal>();
                con.Open();
                using var transaction = con.BeginTransaction();
                var insertData = inputs
                .Where(w => !existsData.Select(s => s.UserId).Contains(w.UserId))
                .Select(s => new UserFinancal
                {
                    UserId = s.UserId!,
                    AccountNo = s.AccountNo,
                    BankName = s.BankName,
                    Currency = "THB",
                    ProvidentFund = s.ProvidentFundPerMonth,
                    Salary = s.Salary,
                    SocialSecurites = s.SocialSecurites,
                    TotalAddiontialExpense = 0,
                    TotalPayment = s.Salary + s.TravelExpense - (s.ProvidentFundPerMonth + s.SocialSecurites),
                    TravelExpenses = s.TravelExpense,

                });
               if(insertData.HasData())
                returnValue = await userFinacialRepository.BulkCreateFinancialData(insertData,con, transaction);
               var updateData = inputs.Where(w => existsData.Select(s => s.UserId).Contains(w.UserId))
                 .Select(s => new UserFinancal
                 {
                     UserId = s.UserId!,
                     AccountNo = s.AccountNo,
                     BankName = s.BankName,
                     Currency = "THB",
                     ProvidentFund = s.ProvidentFundPerMonth,
                     Salary = s.Salary,
                     SocialSecurites = s.SocialSecurites,
                     TotalAddiontialExpense = 0,
                     TotalPayment = s.Salary + s.TravelExpense - (s.ProvidentFundPerMonth + s.SocialSecurites),
                     TravelExpenses = s.TravelExpense,

                 })
                 .ToList();
                if(updateData.HasData())
                {
                    foreach(var data in updateData)
                    {
                        var oldData = existsData.FirstOrDefault(x => x.UserId == data.UserId);
                        data.TotalAddiontialExpense = oldData!.TotalAddiontialExpense;
                        data.TotalPayment = data.Salary + data.TravelExpenses - (data.ProvidentFund + data.SocialSecurites + data.TotalAddiontialExpense);
                        returnValue += await userFinacialRepository.UpdateFinacialData(data, con, transaction) ? 1:0;
                    }
                }
                transaction.Commit();
                con.Close();
                return returnValue;
            });
    }
}
