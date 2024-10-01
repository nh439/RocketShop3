using FluentAssertions;
using LanguageExt;
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
         Mock<UserManager<User>> MockUserManager(List<User> ls)
        {
            var store = new Mock<IUserStore<User>>();
            var mgr = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<User>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<User>());

            mgr.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((string id) => ls.FirstOrDefault(x=>x.Id==id));

            mgr.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string userName) => ls.FirstOrDefault(x => x.UserName ==userName));

            mgr.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => ls.FirstOrDefault(x => x.Email == email));

            mgr.Setup(um => um.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(um => um.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(um => um.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(um => um.DeleteAsync(It.IsAny<User>()))
                .ReturnsAsync(IdentityResult.Success);

            mgr.Setup(um => um.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            mgr.Setup(um => um.HasPasswordAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            mgr.Setup(um => um.GeneratePasswordResetTokenAsync(It.IsAny<User>()))
                .ReturnsAsync("token");

            mgr.Setup(um => um.ResetPasswordAsync(It.IsAny<User>(),It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);



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

        UserServices SetUpUserService(IdentityContext context,UserManager<User> userMgr)
        {
            var newUser = new List<User>();
            var userRepository = new UserRepository(context, userMgr);
            var userInfoRepository = new UserInformationRepository(context);
            var changePasswordHistoryRepository = new ChangePasswordHistoryRepository(context);
            var finacialRepository = new UserFinacialRepository(context);
            var providentFundRepository = new ProvidentRepository(context);
            var logger = MockLogger().Object;
            var userService = new UserServices(logger, userRepository, userInfoRepository,changePasswordHistoryRepository,finacialRepository,providentFundRepository);
            return userService;
        }

        async Task<int> InsertData(IdentityContext context)
        {
            var data = UserInformationGenerator.GenerateFakeData();
            var users = UserGenerator.GenerateFakeData();
            foreach (var item in data) {
                var existsItem = await context.UserInformation.FirstOrDefaultAsync(x => x.UserId == item.UserId);
                if(existsItem.IsNull())
                    context.UserInformation.Add(item);
            }
            foreach (var item in users) {
                var existsItem = await context.Users.FirstOrDefaultAsync(x => x.Id == item.Id);
                if (existsItem.IsNull())
                    context.Users.Add(item);
            }
            return await context.SaveChangesAsync();
        }

        [Fact]
        public async Task UserInformation_Create_Success() {
            var data = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            var userMgr = MockUserManager(new List<User>());
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.CreateInformation(data.FirstOrDefault()!);
            result.GetRight().Should().BeTrue();
        }

        [Fact]
        public async Task UserInformation_Create_Failed() {
            var data = UserInformationGenerator.GenerateFakeData();       
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(new List<User>());
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.CreateInformation(data.FirstOrDefault()!);
            result.GetRight().Should().BeFalse();
        }


        [InlineData("0001")]
        [InlineData("0002")]
        [InlineData("0003")]
        [InlineData("0004")]
        [InlineData("0005")]
        [Theory]
        public async Task Test_UserInformation_Get_HasData(string value)
        {
            var context = SetupContext();
            var inserted= await InsertData(context);
            var userMgr = MockUserManager(new List<User>());
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.GetInformation(value);
            result.GetRight().Extract().Should().NotBeNull();
        }
        [Fact]
        public async Task UserInformation_Update_Success()
        {
            var context = SetupContext();
            var inserted = await InsertData(context);
            var userMgr = MockUserManager(new List<User>());
            var userService = SetUpUserService(context, userMgr.Object);
            var informationToUpdate = await context.UserInformation.FirstOrDefaultAsync(x => x.UserId == "0001");
            informationToUpdate!.CurrentPosition = "SIT";
            var result = await userService.UpdateInformation(informationToUpdate);
            var updatedUser = await context.UserInformation.FirstOrDefaultAsync(x => x.UserId == "0001");
            Assert.True(updatedUser!.CurrentPosition =="SIT");
        }
        [Fact]
        public async Task User_Create_Failed()
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            var inserted = await InsertData(context);
            var userMgr = MockUserManager(users);
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.CreateUser(users.FirstOrDefault()!, information.FirstOrDefault()!);
            Assert.False(result.IsRight && result.GetRight());
        }

        [Fact]
        public async Task User_Create_Success()
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            var userMgr = MockUserManager(new List<User>());
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.CreateUser(users.FirstOrDefault()!, information.FirstOrDefault()!);
            Assert.True(result.IsRight && result.GetRight());
        }
        [Fact]
        public async Task User_ResetPassword_Success()
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(new List<User>());
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.ResetPassword(users.FirstOrDefault()!.Id, "123456");
            Assert.True(result.IsRight && result.GetRight()!);
        }

        [Fact]
        public async Task User_FindById_Found()
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(users);
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.FindById(users.FirstOrDefault()!.Id);
            result.GetRight().Extract().Should().NotBeNull();
        }
        [Fact]
        public async Task User_FindByEmail_Found()
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(users);
            var userService = SetUpUserService(context, userMgr.Object);
            var email = users.FirstOrDefault()!.Email;
            var result = await userService.FindByEmail(email);
            result.GetRight().Extract().Should().NotBeNull();
        }

        [Fact]
        public async Task User_GetProfile_Found()
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(users);
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.GetProfile(users.FirstOrDefault()!.Id);
            result.GetRight().Extract().Should().NotBeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task User_FindProfile_Found(bool isEmail)
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(users);
            var userService = SetUpUserService(context, userMgr.Object);
            var result = isEmail ?
                await userService.FindProfile(users.FirstOrDefault()!.Email!)
               : await userService.FindProfile(users.FirstOrDefault()!.EmployeeCode);
            result.GetRight().Extract().Should().NotBeNull();
        }
        
        [Theory]
        [InlineData(3,13,0)]
        public async Task User_ListUsers_Found(int page,int per,int expected)
        {
            var users = UserGenerator.GenerateFakeData();
            var information = UserInformationGenerator.GenerateFakeData();
            var context = SetupContext();
            await InsertData(context);
            var userMgr = MockUserManager(users);
            var userService = SetUpUserService(context, userMgr.Object);
            var result = await userService.ListUsers(null,page, per);
            Assert.True(result.GetRight()!.Count == expected);
        }


    }
}
