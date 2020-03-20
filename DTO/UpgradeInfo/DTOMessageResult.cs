using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeInfo
{
    public class DTOEditMessageResult
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        public string HandleResult { get; set; }
    }
}
