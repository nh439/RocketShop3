using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Constraint
{
    public static class FileContentTypeConstraint
    {
        public const string Spreadsheet = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Image = "image/jpeg";
        public const string Text = "Text";
        public const string Pdf = "application/pdf";
        public const string Zip = "application/zip";
        public const string Json = "application/json";
        public const string Csv = "text/csv";

    }
}
