using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse.Views
{
    public class AddressView
    {
        public int Id { get; set; } //SubdistrictCode
        public string? ProvinceNameTH {  get; set; }
        public string? DistrictNameTH {  get; set; }
        public string? SubDistrictNameTH {  get; set; }
        public string? ProvinceNameEN {  get; set; }
        public string? DistrictNameEN {  get; set; }
        public string? SubDistrictNameEN {  get; set; }
        public int? PostalCode { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int ProvinceId { get; set; }
        public int ProvinceCode { get; set; }
        public int DistrictId { get; set; }
        public int DistrictCode { get; set; }
        public int SubDistrictId { get; set; }
        public override string ToString() =>
            $"{SubDistrictNameTH} {DistrictNameTH} {ProvinceNameTH} {PostalCode}";
     
    }
}
