using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse
{
    [Table(TableConstraint.District)]
    public sealed class District
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int ProvinceId { get; set; }
        [StringLength(150)]
        public string NameTH { get; set; }
        [StringLength(150)]
        public string NameEN { get; set; }
    }
}
