using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseListSubdistrictRequest : WarehouseQuery
    {
        public WarehouseListSubdistrictRequest(
            string? search=null,
            int? districtId = null,
            int? postalCode = null
            ) :
            base("listSubDistricts",
                new string[]
                {
                    "id",
"districtId",
"code",
"nameTH",
"nameEN",
"postalCode",
"latitude",
"longitude"
                }
               )
        {
            if(search.HasMessage())
                AddParameter(nameof(search), search!);
            if(postalCode.HasValue)
                AddParameter(nameof(postalCode), postalCode.Value);
            if( districtId.HasValue)
                AddParameter(nameof(postalCode), districtId.Value);
        }
    }
}
