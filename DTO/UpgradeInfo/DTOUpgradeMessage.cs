using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeInfo
{
    public class DTOGetUpgradeMessages : BaseRequest
    {
        public Guid UpgradeInfoItemID { get; set; }
    }

    public class DTOBackUpgradeMessages : ReturnResponse
    {
        public List<UpgradeMessage> rows { get; set; }
    }

    /// <summary>
    /// 升级消息
    /// </summary>
    public class UpgradeMessage
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurTime { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }
    }
}
