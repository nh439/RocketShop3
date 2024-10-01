using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.Role)]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public string RoleName {get; set; }
        //FIXTURE SHOP
        public bool CreateFixtureRequest { get; set; } = true;
        public bool EndFixtureRequest { get; set; } = true;
        public bool UpdateFixture { get; set; } = true;
        public bool ViewAnotherUserFixtureData { get; set; } = true;
        //Product Data
        public bool CreateProduct { get; set; } = true;
        public bool UpdateProduct { get; set; } = true;
        public bool DeleteProduct { get; set; } = true;
        //SELLER SHOP
        public bool Sell { get; set; } = true;
        public bool SellSpeicalProduct { get; set; } = true;
        public bool SellerProductManagement { get; set; } = true;
        public bool ViewAnotherSalesValues { get; set; } = true;
        public bool ViewSaleData { get; set; } = true;
        //HR
        public bool CreateEmployee { get; set; } = true;
        public bool UpdateEmployee { get; set; } = true;
        public bool SetResign { get; set; } = true;
        public bool ViewEmployeeDeepData { get; set; } = true;
        public bool HRFinancial { get; set; } = true;
        public bool HRAuditLog { get; set; } = true;
        //ADMIN
        public bool ApplicationAdmin { get; set; } = true;
    }
}
