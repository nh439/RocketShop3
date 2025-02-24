﻿using LanguageExt;
using Npgsql;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;
using System.Diagnostics.CodeAnalysis;

namespace RocketShop.HR.Services
{
    public interface IUserServices
    {
        Task<Either<Exception, bool>> CreateUser(User user, UserInformation information);
        Task<Either<Exception, bool>> UpdateUser(User user, UserInformation information);
        Task<Either<Exception, bool>> DeleteUser(string userId);
        Task<Either<Exception, bool>> ResetPassword(string userId, string newPassword);
        Task<Either<Exception, Option<User>>> FindById(string userId);
        Task<Either<Exception, Option<UserProfile>>> GetProfile(string userId);
        Task<Either<Exception, Option<UserProfile>>> FindProfile(string employeeCodeOrEmail);
        Task<Either<Exception, List<UserView>>> ListUsers(string? searchTerm = null,int? page = null,int per = 20);
        Task<Either<Exception, List<UserView>>> ListByDepartment(string department,int? page = null,int per = 20);
        Task<Either<Exception, List<UserView>>> ListByManager(string managerId,int? page = null,int per = 20);
        Task<Either<Exception, int>> GetCount(string? searchTerm = null);
        Task<Either<Exception, int>> GetLastpage(string? searchTerm = null, int per = 20);
        Task<Either<Exception, int>> GetCountByDepartment(string department);
        Task<Either<Exception, int>> GetLastpageByDepartment(string department, int per = 20);
        Task<Either<Exception, int>> GetCountByManager(string manager);
        Task<Either<Exception, int>> GetLastpageByManager(string manager, int per = 20);
        Task<Either<Exception, bool>> CreateInformation(UserInformation information);
        Task<Either<Exception, bool>> UpdateInformation(UserInformation information);
        Task<Either<Exception, Option<UserInformation>>> GetInformation(string userId);
        Task<Either<Exception, Option<User>>> FindByEmail(string email);
        Task<Either<Exception, Option<UserView>>> GetUserViewById(string userId);
        Task<Either<Exception, bool>> UnlockUser(string userId);
        Task<Either<Exception, bool>> LockUser(string userId, int lockedHours);
        Task<Either<Exception, List<UserView>>> ListUserByUserIdIn(params string[] userIds);
        Task<Either<Exception, List<UserView>>> ListActiveUsers(string? searchTerm = null, int? page = null, int per = 20,bool hasFinancialData = false);
    }
    public class UserServices(
        ILogger<UserServices> logger,
        IConfiguration configuration,
        UserRepository userRepository,
        UserInformationRepository userInformationRepository,
        ChangePasswordHistoryRepository changePasswordHistoryRepository,
        UserFinacialRepository userFinacialRepository,
        ProvidentRepository userProvidentFundRepository) : BaseServices<UserServices>("User Service", logger,new NpgsqlConnection(configuration.GetIdentityConnectionString())), IUserServices
    {
        public async Task<Either<Exception, bool>> CreateUser(User user, UserInformation information) =>
            await InvokeServiceAsync(async () =>
            {
                var createResult = await userRepository.Create(user);
                if (!createResult.Succeeded) return false;
                return await userInformationRepository.Create(information);
            });
        [ExcludeFromCodeCoverage]
        public async Task<Either<Exception, bool>> UpdateUser(User user, UserInformation information) =>
            await InvokeServiceAsync(async () =>
            {
                var updateResult = await userRepository.Update(user);
                if (!updateResult.Succeeded) return false;
                return await userInformationRepository.Update(information);
            });
        [ExcludeFromCodeCoverage]
        public async Task<Either<Exception, bool>> DeleteUser(string userId) =>
            await InvokeDapperServiceAsync(async con =>
            {
                var user = await userRepository.FindById(userId);
                if (user.IsNone) return false;  
                var deleteResult = await userRepository.Delete(user.Extract()!);
                if (!deleteResult.Succeeded) return false;
                con.Open();
                using var transaction = con.BeginTransaction();
                await userProvidentFundRepository.Delete(userId, con, transaction);
                await userFinacialRepository.DeleteFinacialData(userId, con, transaction);
                await userInformationRepository.Delete(userId, con, transaction);
                transaction.Commit();
                return true;
            });

        public async Task<Either<Exception, bool>> ResetPassword(string userId, string newPassword) =>
            await InvokeServiceAsync(async () =>
            {
                var result = await userRepository.ResetPassword(userId, newPassword);
                if (result.Succeeded)
                {
                    return await changePasswordHistoryRepository.CreateHistories(new ChangePasswordHistory
                    {
                        ChangeAt = DateTime.UtcNow,
                        Reset = true,
                        UserId = userId
                    });

                }
                return false;
            } );

        public async Task<Either<Exception, Option<User>>> FindById(string userId) =>
            await InvokeServiceAsync(async () => await userRepository.FindById(userId));

         public async Task<Either<Exception, Option<User>>> FindByEmail(string email) =>
            await InvokeServiceAsync(async () => await userRepository.FindByEmail(email));

        public async Task<Either<Exception, Option<UserProfile>>> GetProfile(string userId) =>
            await InvokeServiceAsync(async () =>
            {
                var userOpt = await userRepository.FindById(userId);
                var info = await userInformationRepository.GetInformation(userId);
                if (userOpt.IsNone)
                    return Option<UserProfile>.None;
                var user = userOpt.Extract();
                return new UserProfile(userId,
                    user!.EmployeeCode,
                    user.Firstname,
                    user.Surname,
                    user.Email,
                    user.ProviderName,
                    user.ProviderKey,
                    info.Extract());
            });

        public async Task<Either<Exception, Option<UserProfile>>> FindProfile(string employeeCodeOrEmail) =>
            await InvokeServiceAsync(async () =>
            {
                var userOpt = await userRepository.FindByEmployeeCode(employeeCodeOrEmail);
                if (userOpt.IsNone)
                    userOpt = await userRepository.FindByEmail(employeeCodeOrEmail);
                if (userOpt.IsNone)
                    return Option<UserProfile>.None;
                var user = userOpt.Extract();
                var info = await userInformationRepository.GetInformation(user!.Id);
                return new UserProfile(user.Id,
                    user!.EmployeeCode,
                    user.Firstname,
                    user.Surname,
                    user.Email,
                    user.ProviderName,
                    user.ProviderKey,
                    info.Extract());
            });

        public async Task<Either<Exception, List<UserView>>> ListUsers(string? searchTerm = null,
            int? page = null,
            int per = 20) =>
            await InvokeServiceAsync(async () => await userRepository.GetUsers(searchTerm, page, per));

        public async Task<Either<Exception, List<UserView>>> ListByDepartment(string department,
            int? page = null,
           int per = 20) =>
           await InvokeServiceAsync(async () => await userRepository.GetByDepartment(department, page, per));

        public async Task<Either<Exception, List<UserView>>> ListByManager(string managerId,
                   int? page = null,
                  int per = 20) =>
                  await InvokeServiceAsync(async () => await userRepository.GetByManager(managerId, page, per));

        public async Task<Either<Exception,int>> GetCount(string? searchTerm = null)=>
            await InvokeServiceAsync(async ()=> await userRepository.GetCount(searchTerm));

        public async Task<Either<Exception, int>> GetLastpage(string? searchTerm = null,int per = 20) =>
            await InvokeServiceAsync(async () => await userRepository.GetLastpage(searchTerm,per));

        public async Task<Either<Exception,int>> GetCountByDepartment(string department)=>
            await InvokeServiceAsync(async ()=> await userRepository.GetCountByDepartment(department));

        public async Task<Either<Exception, int>> GetLastpageByDepartment(string department,int per = 20) =>
            await InvokeServiceAsync(async () => await userRepository.GetLastpageByDepartment(department,per));

         public async Task<Either<Exception,int>> GetCountByManager(string manager)=>
            await InvokeServiceAsync(async ()=> await userRepository.GetCountByManager(manager));

        public async Task<Either<Exception, int>> GetLastpageByManager(string manager, int per = 20) =>
            await InvokeServiceAsync(async () => await userRepository.GetLastpageByManager(manager,per));



        public async Task<Either<Exception, bool>> CreateInformation(UserInformation information) =>
            await InvokeServiceAsync(async () => await userInformationRepository.Create(information));

        public async Task<Either<Exception, bool>> UpdateInformation(UserInformation information) =>
            await InvokeServiceAsync(async () => await userInformationRepository.Update(information));
    
        public async Task<Either<Exception,Option<UserInformation>>> GetInformation(string userId)=>
            await InvokeServiceAsync(async () => await userInformationRepository.GetInformation(userId));

        public async Task<Either<Exception, Option<UserView>>> GetUserViewById(string userId)=>
            await InvokeServiceAsync(async () => (await userRepository.GetUserViewById(userId)).AsOptional());

        public async Task<Either<Exception, bool>> UnlockUser(string userId) =>
            await InvokeServiceAsync(async () => await userRepository.UnlockUser(userId));

        public async Task<Either<Exception, bool>> LockUser(string userId, int lockedHours) =>
            await InvokeServiceAsync(async () => await userRepository.LockedUser(userId,DateTime.UtcNow.AddHours(lockedHours)));

        public async Task<Either<Exception, List<UserView>>> ListUserByUserIdIn(params string[] userIds)=>
            await InvokeServiceAsync(async () =>await userRepository.ListUserIn(userIds));

        public async Task<Either<Exception, List<UserView>>> ListActiveUsers(string? searchTerm = null, int? page = null, int per = 20, bool hasFinancialData = false) =>
            await InvokeServiceAsync(async () => {
               var users = await userRepository.GetActiveUsers(searchTerm, page, per);
                if (!hasFinancialData)
                    return users;
                var financialData = await userFinacialRepository.ListFinancialDataByUserIn(users.Select(s => s.UserId));
                return users.Where(x=>financialData.Select(s=>s.UserId).Contains(x.UserId)).ToList();
                });
    }
}
