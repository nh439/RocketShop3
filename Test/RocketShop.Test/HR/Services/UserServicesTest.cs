using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using RocketShop.HR.Repository;
using RocketShop.HR.Services;
using RocketShop.Test.Context;
using RocketShop.Test.Data;
using Serilog;

namespace RocketShop.Test.HR.Services
{
    public class UserServicesTest
    {
        public IdentityContext SetupContext()
        {
            var inMemorySettings = new Dictionary<string, string> {
    {"TopLevelKey", "TopLevelValue"},
    {"SectionName:SomeKey", "SectionValue"},
    //...populate as needed for the test
};

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings!)
                .Build();

            IdentityContext context =  new IdentityContextTest(configuration);
            context.Database.EnsureCreated();
            return context;
        }
         Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        Mock<ILogger> MockLogger()
        {
            var logger = new Mock<ILogger>();
            logger.Setup(x => x.Verbose(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(x => x.Information(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(x => x.Information(It.IsAny<string>(), It.IsAny<string>()));
            logger.Setup(x => x.Error(It.IsAny<Exception>(), It.IsAny<string>()));
            logger.Setup(x => x.Error(It.IsAny<string>(), It.IsAny<string>()));
            logger.Setup(x => x.Error(It.IsAny<string>()));
            logger.Setup(x => x.ForContext<object>()).Returns(logger.Object);
            logger.Setup(x => x.ForContext(It.IsAny<string>(), It.IsAny<object>(), false)).Returns(logger.Object);

            return logger;
        }

        async Task<int> InsertData(IdentityContext context)
        {
            var data = UserInformationGenerator.GenerateFakeData();
            await context.AddAsync(data);
            return await context.SaveChangesAsync();
        }

        [Fact]
        public async Task Test_UserInformation_Create() {
            var data = UserInformationGenerator.GenerateFakeData();
            var newUser = new List<User>();
            var context = SetupContext();
            var userMgr = MockUserManager<User>(newUser);
            var userRepository = new UserRepository(context, userMgr.Object);
            var userInfoRepository = new UserInformationRepository(context);
            var logger = MockLogger().Object;
            var userService = new UserServices(logger, userRepository, userInfoRepository);
            var result = await userService.CreateInformation(data.FirstOrDefault()!);
            result.GetRight().Should().BeTrue();
        }
        [InlineData("0001")]
        [InlineData("0002")]
        [InlineData("0003")]
        [InlineData("0004")]
        [InlineData("0005")]
        [Theory]
        public async Task Test_UserInformation_Get_HasData(string value)
        {
            var newUser = new List<User>();
            var context = SetupContext();
            var inserted= await InsertData(context);
            var s = context.UserInformation.ToList();
            var userMgr = MockUserManager<User>(newUser);
            var userRepository = new UserRepository(context, userMgr.Object);
            var userInfoRepository = new UserInformationRepository(context);
            var logger = MockLogger().Object;
            var userService = new UserServices(logger, userRepository, userInfoRepository);
            var result = await userService.GetInformation(value);
            result.GetRight().Extract().Should().NotBeNull();
        }
    }
}
