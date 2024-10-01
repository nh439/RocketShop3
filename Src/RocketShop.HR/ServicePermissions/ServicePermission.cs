using RocketShop.Database.Model.Identity;

namespace RocketShop.HR.ServicePermissions
{
    public static class ServicePermission
    {
        public const string AllHRServiceName = "AllHRServiceName";
        public const string HREmployeeName = "HREmployee";
        public const string HRFinancialName = "HRFinancial";
        public const string HRAuditName = "HRAudit";
        public const string AppAdminName = "AppAdmin";
        public static string[] AllHRService =
            [
            nameof(Role.UpdateEmployee),
            nameof(Role.CreateEmployee),
            nameof(Role.ViewEmployeeDeepData),
            nameof(Role.SetResign),
            nameof(Role.HRAuditLog),
            nameof(Role.HRFinancial)
            ]; 
        public static string[] HREmployee =
            [
            nameof(Role.UpdateEmployee),
            nameof(Role.CreateEmployee),
            nameof(Role.ViewEmployeeDeepData),
            nameof(Role.SetResign),
            ];
        public static string[] HRAuditLog =
            [
            nameof(Role.HRAuditLog)
            ];
        public static string[] HRFinancial =
            [
            nameof(Role.HRFinancial)
            ];
        public static string[] ApplicationAdmin =
            [
            nameof(Role.ApplicationAdmin)
            ];

    }
}
