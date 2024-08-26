using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
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
    public class PasswordServices : BaseServices<PasswordServices>, IPasswordServices
    {
        readonly UserManager<User> _userManager;
        readonly DbSet<ChangePasswordHistory> _changePasswordHistory;
        public PasswordServices(UserManager<User> userManager, ILogger<PasswordServices> logger, IdentityContext context) : base("Password", logger)
        {
            _userManager = userManager;
            _changePasswordHistory = context.ChangePasswordHistory;
        }

        public async Task<Either<Exception, IdentityResult>> ChangePassword(string userId, string oldPassword, string newPassword) =>
            await InvokeServiceAsync(async () =>
            {
                var identityResult = await _userManager.ChangePasswordAsync((await _userManager.FindByIdAsync(userId))!, oldPassword, newPassword);
                if (identityResult.Succeeded)
                    await _changePasswordHistory.Add(new ChangePasswordHistory
                    {
                        ChangeAt = DateTime.UtcNow,
                        Reset = false,
                        UserId = userId
                    })
                    .Context
                    .SaveChangesAsync();

                return identityResult;
            });

        public async Task<Either<Exception, IdentityResult>> ResetPassword(string userId, string token, string password) =>
            await InvokeServiceAsync(async () =>
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    throw new NullReferenceException("User Invaild");
                var result = await _userManager.ResetPasswordAsync(user, token, password);
                if (result.Succeeded)
                    await _changePasswordHistory.Add(new ChangePasswordHistory
                    {
                        ChangeAt = DateTime.UtcNow,
                        Id = userId,
                        Reset = true,
                        UserId = userId
                    })
     .Context
     .SaveChangesAsync();

                return result;
            });

        public async Task<Either<Exception, string>> GenerateResetPasswordToken(string userId) =>
            await InvokeServiceAsync(async () => await _userManager.GeneratePasswordResetTokenAsync((await _userManager.FindByIdAsync(userId))!));
    }
}
