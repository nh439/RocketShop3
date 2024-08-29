using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Enum;
using RocketShop.HR.Repository;

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
                    return new UserFinacialData(finacial!, expenses,providentFund).AsOptional();
                }
                return new UserFinacialData(finacial!,null, providentFund).AsOptional();
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
                return await userFinacialRepository.UpdateProvidentFundRate(userId, newProvidentFundRate, item.TotalPayment,con);
            });

    }
}
