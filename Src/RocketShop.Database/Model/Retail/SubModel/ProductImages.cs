using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Retail.SubModel
{
    [Table(TableConstraint.ProductImage)]
    public class ProductImages
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ImagePath { get; set; }
    }
}
