using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse
{
    [Table(TableConstraint.SubDistrict)]
    public sealed class SubDistrict
    {
        [Key]
        public int Id { get; set; }
        public int DistrictId { get; set; }
        public int Code { get; set; }
        public string? NameTH { get; set; }
        public string? NameEN { get; set; }
        public int? PostalCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
