using RocketShop.Database.Model.Retail.SubModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Retail
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    [Table(TableConstraint.Product)]
    public class Product
    {
        [Key]
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public long? SubCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }       
        public string? Thumbnails { get; set; }
        public List<ProductAttribute>? Attributes { get; set; }
        public List<ProductImages> Images { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
