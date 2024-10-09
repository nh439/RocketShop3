﻿using LanguageExt;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IUserReportServices
    {
        Task<Either<Exception, List<EmployeeDataReport>>> ListReport(string? keyword = null, int? page = null, int per = 100);
    }
    public class UserReportServices(ILogger<UserReportServices> logger,
        UserReportRepository repository) : 
        BaseServices<UserReportServices>("User Report Service", logger),IUserReportServices
    {
        public async Task< Either<Exception,List<EmployeeDataReport>>> ListReport(string? keyword = null,int? page= null,int per =100) =>
            await InvokeServiceAsync(async ()=> await repository.ListEmployeeDataReport(keyword,page,per));

        public async Task<Either<Exception,int>> GetReportCount() =>
            await InvokeServiceAsync(async () => await repository.GetEmployeeDataReportCount());
    }
}
