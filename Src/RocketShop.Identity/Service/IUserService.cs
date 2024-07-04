using RocketShop.Database.EntityFramework;

namespace RocketShop.Identity.Service
{
    public interface IUserService
    {

    }
    public class UserService : IUserService
    {
        readonly IdentityContext context;
        public UserService() { }
    }
}
