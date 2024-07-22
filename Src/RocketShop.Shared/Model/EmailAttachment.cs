using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model
{
    public class EmailAttachment
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Byte[] Content { get; set; }
        public DateTime DateModified { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime AttachDate { get; set; } = DateTime.Now;
        public long Size { get; set; }
    }
}
