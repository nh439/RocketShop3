using RocketShop.Database.Model.Identity;

namespace RocketShop.Retail.ServicePermission
{
    public static class PolicyNames
    {
        public static string AllSeller = nameof(PolicyPermissions.AllSellerPolicy);
        public static string GeneralSeller = nameof(PolicyPermissions.GeneralSeller);
        public static string SpeicalSeller = nameof(PolicyPermissions.SellerSpeical);
        public static string SellerManager = nameof(PolicyPermissions.SellerManager);

    }
    public static class PolicyPermissions
    {
        public static string[] AllSellerPolicy = new[]
        {
    nameof(Role.Sell),
    nameof(Role.SellSpeicalProduct),
    nameof(Role.SellerProductManagement),
    nameof(Role.ViewSaleData),
    nameof(Role.ViewAnotherSalesValues)
};
        public static string[] GeneralSeller = new[] {
        nameof(Role.Sell),
        nameof(Role.ViewSaleData)
        };
        public static string[] SellerSpeical = new[] {
            nameof(Role.Sell),
        nameof(Role.ViewSaleData),
        nameof(Role.SellSpeicalProduct)
        };
        public static string[] SellerManager = new[] {
               nameof(Role.Sell),
        nameof(Role.ViewSaleData),
        nameof(Role.ViewAnotherSalesValues)
        };
    }
}
