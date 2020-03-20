using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model
{
    [Table("Prc_ConfigSet")]
    public class Prc_ConfigSet: Prc_BaseInfo
    {
        /// <summary>
        /// 配置key
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 配置值
        /// </summary>
        public string Value { get; set; }
    }
}
