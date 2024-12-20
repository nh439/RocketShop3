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
    [Table(TableConstraint.MainCategory)]
    public sealed class MainCategory
    {
        [Key]
        public long Id { get; set; }
        public string? NameTh { get; set; }
        public string NameEn { get; set; }
        public string? Description { get; set; }
        public override string ToString() => NameTh ?? NameEn;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime LastUpdated {  get; set; } = DateTime.UtcNow;
        public string CreateBy { get; set; }
        public string LastUpdatedBy { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
