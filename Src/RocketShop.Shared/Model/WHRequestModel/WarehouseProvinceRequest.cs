using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    internal class WarehouseProvinceRequest : WarehouseQuery
    {
        public WarehouseProvinceRequest(string? search = null) : base(
            "listProvince",
            new string[] {
            "id",
            "code",
            "nameTH",
            "nameEN"
            })
        {
            if (search.HasData())
                AddParameter("search", search!);
        }
    }
}
