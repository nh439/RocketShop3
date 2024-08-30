using RocketShop.Shared.Model.ExcelModel;

namespace RocketShop.HR.LocalModel
{
    public record InputFinacialDataVerify(string EmployeeCode,
        string BankName,
        string AccountNo,
        decimal Salary,
        decimal SocialSecurites,
        decimal TravelExpense,
        decimal ProvidentFundPerMonth,
        string? UserId = null,
        bool IsCorrupt = false) : InputOutputUserFinacialData(EmployeeCode, BankName, AccountNo, Salary, SocialSecurites, TravelExpense, ProvidentFundPerMonth);
}
