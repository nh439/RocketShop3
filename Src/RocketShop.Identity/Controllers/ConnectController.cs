using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.ControllerFunction;

namespace RocketShop.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController(ILogger<ConnectController> logger,
        IConfiguration configuration,
        IdentityContext context) : 
        APIControllerServices(logger)
    {

        [HttpGet] 
        public IActionResult Get() =>
            Respond(() =>
            {
                var query = context.UserFinancal.Join(context.UserProvidentFund,
                    f => f.UserId,
                    p => p.UserId,
                    (f, p) => new UserFinancialView
                    {
                        UserId = f.UserId,
                        AccountNo = f.AccountNo,
                        BanckName = f.BanckName,
                        Currency = f.Currency,
                        ProvidentFundPerMonth = f.ProvidentFund,
                        AccumulatedProvidentFund=p.Balance,
                        Salary = f.Salary,
                        SocialSecurites = f.SocialSecurites,
                        TotalAddiontialExpense=f.TotalAddiontialExpense,
                        TotalPayment=f.TotalPayment,
                        TravelExpenses=f.TravelExpenses
                    });
                return query.ToQueryString();
            });
    }
    
}
class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
}