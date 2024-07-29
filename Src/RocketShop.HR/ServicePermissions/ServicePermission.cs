using RocketShop.Database.Model.Identity;

namespace RocketShop.HR.ServicePermissions
{
    public static class ServicePermission
    {
        public const string AllHRServiceName = "AllHRServiceName";
        public static string[] AllHRService =
            [
            nameof(Role.UpdateEmployee),
            nameof(Role.CreateEmployee),
            nameof(Role.ViewEmployeeDeepData),
            nameof(Role.SetResign)
            ]; 
    }
}
