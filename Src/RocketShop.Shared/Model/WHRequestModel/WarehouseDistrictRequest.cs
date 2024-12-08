using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseDistrictRequest : WarehouseQuery
    {
        public WarehouseDistrictRequest(int id) :
            base("district",
                new string[]
                {
                    "id",
                    "code",
                    "provinceId",
                    "nameTH",
                    "nameEN"                    
                },
                new List<WarehouseQueryParameter>()
                {
                   new WarehouseQueryParameter("id", id),
                })
        { }
    }
}
