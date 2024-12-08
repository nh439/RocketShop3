using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseSubdistrictRequest : WarehouseQuery
    {
        public WarehouseSubdistrictRequest(int id):
            base("subDistrict",
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
                },
                new List<WarehouseQueryParameter>()
                {
                    new WarehouseQueryParameter("id",id)
                })
        {

        }
    }
}
