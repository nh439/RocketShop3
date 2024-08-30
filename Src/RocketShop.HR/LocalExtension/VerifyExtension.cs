using RocketShop.Framework.Extension;
using RocketShop.HR.LocalModel;

namespace RocketShop.HR.LocalExtension
{
    public static class VerifyExtension
    {
        public static bool VerifyFinancialData(this IEnumerable<InputFinacialDataVerify> data) =>
            !data.Where(x => x.IsCorrupt).HasData();

        public static int GetCorrupedFinancialData(this IEnumerable<InputFinacialDataVerify> data) =>
            data.Where(x => x.IsCorrupt).Count();

        public static List<InputFinacialDataVerify> GetCorrupedData(this IEnumerable<InputFinacialDataVerify> data) =>
             data.Where(x => x.IsCorrupt).ToList();
    }
}
