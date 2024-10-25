using LanguageExt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.SharedBlazorServices.Scope
{
    public interface ISharedUserServices
    {
        Task<Either<Exception, User>> GetUser(string subject);
    }
    public class SharedUserServices(IdentityContext context,ILogger<SharedUserServices> logger)
        : BaseServices<SharedUserServices>("Shared User",logger),ISharedUserServices
    {
        public async Task<Either<Exception, User>> GetUser(string subject) =>
            await InvokeServiceAsync(async () => await context.Users.FirstOrDefaultAsync(x => x.Id == subject));
    }
}
