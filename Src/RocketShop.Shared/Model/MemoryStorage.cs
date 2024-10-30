using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model
{
    public class MemoryStorage
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public DateTime CreateDate { get; internal set; } = DateTime.Now;
        public TimeSpan ExpiredIn { get; set; } = TimeSpan.FromMinutes(5);
        public DateTime Expired
        {
            get
            {
                return CreateDate.Add(ExpiredIn);
            }
        }
        public bool IsExpired
        {
            get
            {
                return Expired < DateTime.Now;
            }
        }
    }
}
