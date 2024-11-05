using RocketShop.Database.Model.Identity;

namespace RocketShop.Warehouse.Admin.ServicePermission
{
    public static class PolicyNames
    {
       public const string AppAdminName = "AppAdmin";
    }
    public static class PolicyPermissions
    {
        public static string[] AppAdminPermissions =
            new[] {nameof(Role.ApplicationAdmin) };
    }
}
