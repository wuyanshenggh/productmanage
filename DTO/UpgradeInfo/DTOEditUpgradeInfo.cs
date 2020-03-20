using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeInfo
{
    public class DTOEditUpgradeInfo : BaseRequest
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 确认升级时间
        /// </summary>
        public DateTime ConfirmUpgradeTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Summary { get; set; }
    }
}
