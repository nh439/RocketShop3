using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using System.Diagnostics;

namespace RocketShop.Identity.Service
{
    public interface IProfileServices
    {
        Task<Either<Exception, UserProfile>> GetProfile(string id);
        Task<Either<Exception, bool>> ClearProviderKey(string userId);
    }
    public class ProfileServices(IdentityContext identityContext,
            UserManager<User> userManager,
            Serilog.ILogger logger)
        : BaseServices("Profile Service", logger), 
        IProfileServices
    {

        public async Task<Either<Exception,UserProfile>> GetProfile(string id) =>
            await InvokeServiceAsync(async () =>
            {
                var user = await userManager.FindByIdAsync(id);
                if (user.IsNull())
                    throw new NullReferenceException("User Not Found");
                var information = await identityContext
                .UserInformation.FirstOrDefaultAsync(x => x.UserId == id);
                return new UserProfile(id,
                    user!.EmployeeCode,
                    user.Firstname,
                    user.Surname,
                    user.Email,
                    user.ProviderName,
                    user.ProviderKey,
                    information);
            });
        public async Task<Either<Exception, bool>> ClearProviderKey(string userId) =>
            await InvokeServiceAsync(async () => await identityContext
            .Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(s =>
            s.SetProperty(c => c.ProviderKey, string.Empty)
            .SetProperty(c => c.ProviderName, string.Empty)
            ) > 0
            );
        
    }
}
