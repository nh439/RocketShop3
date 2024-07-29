using RocketShop.Database.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.NonDatabaseModel
{
    public class SlipData :UserPayroll
    {
        public List<AdditionalPayroll>? AdditionalPayrolls { get; set; }
    }
}
