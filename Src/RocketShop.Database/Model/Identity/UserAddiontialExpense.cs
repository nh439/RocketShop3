using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    /// <summary>
    /// ค่าใช้จ่ายเพิ่มเติม
    /// </summary>
    [Table(TableConstraint.UserAddiontialExpense)]
    public class UserAddiontialExpense
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public string ExpenseName { get; set; }
        /// <summary>
        /// จ่ายครั้งเดียว
        /// </summary>
        public bool OneTimePay { get; set; }
        /// <summary>
        /// ถ้าจ่ายครั้งเดียวหรือรายปี ต้องจ่ายเดือนไหน
        /// </summary>
        public int? Month { get; set; }
        /// <summary>
        /// ถ้าจ่ายครั้งเดียว ต้องจ่ายปีไหน
        /// </summary>
        public int? Year { get; set; }
        /// <summary>
        /// รอบการจ่้าย เช่น รายเดือน รายปี หรือ ครั้งเดียว
        /// </summary>
        public string PreiodType { get; set; } = "Monthly";
        /// <summary>
        /// จำนวนเงิน (ถ้าติดลบ จะหักจาก Total Payment)
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// จ่ายไปแล้วยัง(สำหรับใช้รอบครั้งเดียว)
        /// </summary>
        public bool Paid { get; set; }=false;
    }
}
