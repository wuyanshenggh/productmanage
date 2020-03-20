using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model
{
    [Table("Prc_Holiday")]
    public class Prc_Holiday: Prc_BaseInfo
    {
        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 月份与日份的json数据
        /// </summary>
        public string Data { get; set; }
    }
}
