using LanguageExt;
using Microsoft.AspNetCore.Identity;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Services;

namespace RocketShop.Identity.Service
{
    public interface IPasswordServices
    {
        Task<Either<Exception, IdentityResult>> ChangePassword(string userId, string oldPassword, string newPassword);
        Task<Either<Exception, IdentityResult>> ResetPassword(string userId, string token, string password);
        Task<Either<Exception, string>> GenerateResetPasswordToken(string userId);
    }
    public class PasswordServices : BaseServices, IPasswordServices
    {
        readonly UserManager<User> _userManager;
        public PasswordServices(UserManager<User> userManager,Serilog.ILogger logger) : base("Password", logger)
        {
            _userManager = userManager;
        }

        public async Task<Either<Exception, IdentityResult>> ChangePassword(string userId, string oldPassword, string newPassword) =>
            await InvokeServiceAsync(async () => await _userManager.ChangePasswordAsync((await _userManager.FindByIdAsync(userId))!, oldPassword, newPassword));

        public async Task<Either<Exception, IdentityResult>> ResetPassword(string userId, string token, string password) =>
            await InvokeServiceAsync(async () =>
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NullReferenceException("User Invaild");
                return await _userManager.ResetPasswordAsync(user, token, password);
            });

        public async Task<Either<Exception, string>> GenerateResetPasswordToken(string userId) =>
            await InvokeServiceAsync(async () => await _userManager.GeneratePasswordResetTokenAsync((await _userManager.FindByIdAsync(userId))!));
    }
}
