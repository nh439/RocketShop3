using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseListDistrictRequest : WarehouseQuery
    {
        public WarehouseListDistrictRequest(int? provinceId = null,string? search = null) :
            base("district",
                new string[]
                {
                    "id",
                    "code",
                    "provinceId",
                    "nameTH",
                    "nameEN"                    
                })
               
        {
            if (provinceId.HasValue)
                AddParameter("provinceId", provinceId);
            if(search.HasMessage())
                AddParameter("search",search!);
        }
    }
}
