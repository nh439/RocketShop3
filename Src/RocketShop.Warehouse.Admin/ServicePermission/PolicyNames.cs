using RocketShop.Database.Model.Identity;

namespace RocketShop.Warehouse.Admin.ServicePermission
{
    public static class PolicyNames
    {
        public const string AppCredentialManagerName = "AppAdmin";
        public const string DataMaintainerName = "DataMaintainer";
        public const string AnyWHPolicyName = "Any";

    }
    public static class PolicyPermissions
    {
        public static string[] AppCredentialManagerPermissions =
            new[] {
                nameof(Role.ApplicationAdmin),
                nameof(Role.WHManageCredential)
            };
        public static string[] DataMaintainerPermissions =
            new[] {
                nameof(Role.ApplicationAdmin),
                nameof(Role.WHCollectionMaintainer)
            };
         public static string[] AnyWHPermissions =
            new[] {
                nameof(Role.ApplicationAdmin),
                nameof(Role.WHCollectionMaintainer),
                nameof(Role.WHManageCredential)
            };


    }
}
