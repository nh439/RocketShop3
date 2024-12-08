using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseListProvinceRequest : WarehouseQuery
    {
        public WarehouseListProvinceRequest(int id) : base(
            "listProvince",
            new string[] {
            "id",
            "code",
            "nameTH",
            "nameEN"
            },
            new List<WarehouseQueryParameter>
            {
                new WarehouseQueryParameter("id",id)
            })
        {
            
        }
    }
}
