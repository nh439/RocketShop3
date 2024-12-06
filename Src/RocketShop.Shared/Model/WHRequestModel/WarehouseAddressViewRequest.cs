using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model.WHRequestModel
{
    public class WarehouseAddressViewRequest : WarehouseQuery

    {
        public WarehouseAddressViewRequest(
            string? search = null,
            int? page = null,
            int? pagesize = null) : base(
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
"subDistrictId"})
        {
            if (search.HasMessage())
                AddParameter("search", search!);
            if (page.HasValue)
                AddParameter("page", page.Value);
            if (pagesize.HasValue)
                AddParameter("page", pagesize.Value);
        }
    }
}
