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
    }
    public class ProfileServices: RocketShop.Framework.Services.BaseServices, IProfileServices
    {
        readonly IdentityContext _identityContext;
        readonly UserManager<User> _userManager;
        public ProfileServices(IdentityContext identityContext, 
            UserManager<User> userManager,
            Serilog.ILogger logger)
            :base("Profile Service",logger)
        {
            this._identityContext = identityContext;
            this._userManager = userManager;
        }
        public async Task<Either<Exception,UserProfile>> GetProfile(string id) =>
            await InvokeServiceAsync(async () =>
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user.IsNull())
                    throw new NullReferenceException("User Not Found");
                var information = await _identityContext
                .UserInformation.FirstOrDefaultAsync(x => x.UserId == id);
                return new UserProfile(id,
                    user!.EmployeeCode,
                    user.Firstname,
                    user.Surname,
                    user.Email,
                    information);
            });
        
    }
}
