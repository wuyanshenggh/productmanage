using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeInfo
{
    /// <summary>
    /// 升级详情
    /// </summary>
    public class DTOGetUpgradeDetail : BaseRequest
    {
        public Guid UpgradeInfoID { get; set; }
    }

    public class DTOBackUpgradeDetail : ReturnResponse
    {
        public List<UpgradeDetail> rows { get; set; }
    }

    public class UpgradeDetail
    {
        /// <summary>
        /// UpgradeMessage表ID
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 升级细项ID
        /// </summary>
        public Guid UpgradeInfoItemID { get; set; }

        /// <summary>
        /// 商户类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime? OccurTime { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 消息标志
        /// </summary>
        public string MessageFlag { get; set; }

        /// <summary>
        /// 耗时
        /// </summary>
        public int? TimeConsum { get; set; }

        /// <summary>
        /// 心跳状态(0正常，1异常，-1无)
        /// </summary>
        public int HeartbeatStatus { get; set; }

        /// <summary>
        /// 处理状态，详见EnumHandleStatus
        /// </summary>
        public int HandleStatus { get; set; }

        /// <summary>
        /// 处理选项集合
        /// </summary>
        public List<string> HandleOptions { get; set; }

        /// <summary>
        /// 消息结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 升级状态
        /// </summary>
        public int UpgradeStatus { get; set; }
    }
}
