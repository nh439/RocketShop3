using LanguageExt;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.NonDatabaseModel;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.HR.Repository;

namespace RocketShop.HR.Services
{
    public interface IUserReportServices
    {
        Task<Either<Exception, List<EmployeeDataReport>>> ListReport(string? keyword = null, int? page = null, int per = 100);
        Task<Either<Exception, int>> GetReportCount(string? keyword = null);
        Task<Either<Exception, int>> GetReportLastpage(string? keyword = null, int per = 100);
        Task<Either<Exception, List<EmployeeMatrixReport>>> ListUserMatrixReport(string? keyword = null, int? page = null, int per = 100);
    }
    public class UserReportServices(ILogger<UserReportServices> logger,
        UserReportRepository repository,
        RoleRepository roleRepository) : 
        BaseServices<UserReportServices>("User Report Service", logger),IUserReportServices
    {
        public async Task< Either<Exception,List<EmployeeDataReport>>> ListReport(string? keyword = null,int? page= null,int per =100) =>
            await InvokeServiceAsync(async ()=> await repository.ListEmployeeDataReport(keyword,page,per));

        public async Task<Either<Exception,int>> GetReportCount(string? keyword = null) =>
            await InvokeServiceAsync(async () => await repository.GetEmployeeDataReportCount(keyword));

         public async Task<Either<Exception,int>> GetReportLastpage(string? keyword = null,int per =100) =>
            await InvokeServiceAsync(async () => await repository.GetEmployeeDataReportLastpage(keyword,per));

        public async Task<Either<Exception,List<EmployeeMatrixReport>>> ListUserMatrixReport(string? keyword = null, int? page = null, int per = 100)
        {
            List<EmployeeMatrixReport> returnValue = new();
            var users = await repository.ListEmployeeDataReport(keyword, page, per);
            var roleData = await roleRepository.GetRoleByUserIdRange(users.Select(s => s.UserId));
            var roleProp = typeof(Role).GetProperties();
            foreach (var user in users)
            {
                var assignedRole = roleData.Where(x=>x.Item1==user.UserId).Select(x=>x.Item2).FirstOrDefault();
                List<string> permissions = new List<string>();
                assignedRole.HasDataAndForEach(f =>
                {
                    roleProp.HasDataAndForEach(prop =>
                    {
                        if (prop.PropertyType == typeof(bool))
                        {
                          var value = prop.GetValue(f).IsNotNull() ?(bool)prop.GetValue(f)! : false;
                            if(value)
                                permissions.Add(prop.Name);
                        }
                    });
                });
                returnValue.Add(new EmployeeMatrixReport(user, assignedRole?.Select(s => s.RoleName), permissions.Distinct()));
            }
            return returnValue;
        }
    }
}
