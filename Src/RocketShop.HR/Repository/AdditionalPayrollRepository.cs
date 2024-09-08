using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.Extension;
using System.Data;

namespace RocketShop.HR.Repository
{
    public class AdditionalPayrollRepository(IdentityContext context)
    {
        const string tableName = TableConstraint.AdditionalPayroll;
        readonly DbSet<AdditionalPayroll> entity = context.AdditionalPayroll;

        public async Task<List<AdditionalPayroll>> GetAdditionalPayrolls(string payrollId)=>
            await entity.Where(x=>x.PayrollId == payrollId).ToListAsync();

        public async Task<bool> SetAdditionalPayroll(string payrollId,IEnumerable<AdditionalPayroll> additionalPayrolls,IDbConnection identityConnection,IDbTransaction? transaction = null)
        {
            bool transactionInjected = transaction.IsNotNull();
            if (!transactionInjected)
            {
                if(identityConnection.State != ConnectionState.Open)
                    identityConnection.Open();
                transaction = identityConnection.BeginTransaction();
            }
            try
            {
                await identityConnection.CreateQueryStore(tableName)
                    .Where(nameof(AdditionalPayroll.PayrollId), payrollId)
                    .DeleteAsync(transaction);
                if (additionalPayrolls.HasData())
                {
                    additionalPayrolls.HasDataAndForEach(f => f.PayrollId = payrollId);
                    await identityConnection.CreateQueryStore(tableName)
                        .BulkInsertAsync(additionalPayrolls,transaction);
                }
                if (!transactionInjected)
                    transaction!.Commit();
                return true;
            }
            catch {
                if (!transactionInjected)
                    transaction!.Rollback();
                throw;
            }
            finally
            {
                if (!transactionInjected)
                {
                    transaction?.Dispose();
                    if (identityConnection.State != ConnectionState.Closed)
                        identityConnection.Close();
                }
            }
    }
}
