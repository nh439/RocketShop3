using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RocketShop.Framework.ControllerFunction;

namespace RocketShop.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectController : APIControllerServices
    {
        readonly ILogger<ConnectController> logger;
        public ConnectController(ILogger<ConnectController> logger) :
            base(logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult Get() =>
            Respond(() =>
            {
                List<Department> departments = new List<Department>();
                IQueryable<Department> query = departments.AsQueryable();
                var s = query.Where(x => x.Name == "IT").ToQueryString();
                return s;
            });
    }
    
}
class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
}