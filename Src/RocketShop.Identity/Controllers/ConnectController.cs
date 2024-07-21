using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using RocketShop.Database;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework.QueryGenerator;
using RocketShop.Framework.ControllerFunction;

namespace RocketShop.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController(ILogger<ConnectController> logger,
        IConfiguration configuration) : 
        APIControllerServices(logger)
    {

        [HttpGet] 
        public IActionResult Get() =>
            Respond(() =>
            {
                using var conn = new NpgsqlConnection(configuration.GetIdentityConnectionString());
                var query = conn.CreateQueryStore(TableConstraint.UserInformation)
                .Where(x => x.Where("UserId", "81c6bf5b-aac5-4f72-a2af-86d4de4de935").OrWhere("UserId", "TTT"))
                .OrWhere("Department","IT");
                var sql = query.Compiled();
                var item = query.Fetch<UserInformation>();
                return Ok( item);
            });
    }
    
}
class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
}