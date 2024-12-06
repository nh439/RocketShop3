using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseAddressRequest : WarehouseQuery

    {
        public WarehouseAddressRequest(
            int id) : base(
            "AddressView",
            new string[] { "id",
"provinceNameTH",
"districtNameTH",
"subDistrictNameTH",
"provinceNameEN",
"districtNameEN",
"subDistrictNameEN",
"postalCode",
"latitude",
"longitude",
"provinceId",
"provinceCode",
"districtId",
"districtCode",
"subDistrictId"},
            new List<WarehouseQueryParameter>()
            {
                new WarehouseQueryParameter("id",id)
            })
        {
            
        }
    }
}
