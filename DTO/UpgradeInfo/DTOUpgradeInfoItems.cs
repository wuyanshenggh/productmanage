using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeInfo
{
    public class DTOGetUpgradeInfoItem : BaseRequest
    {
        public Guid UpgradeInfoID { get; set; }
    }

    
    public class DTOBackUpgradeInfoItem : ReturnResponse
    {
        /// <summary>
        /// 整个任务的更新状态,详见EnumUpgradeStatus
        /// </summary>
        public int InfoUpgradeStatus { get; set; }

        public List<UpgradeInfoItem> rows { get; set; }
    }

    /// <summary>
    /// 升级细项
    /// </summary>
    public class UpgradeInfoItem
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 商户类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 网络状态(0正常，1异常，-1无)
        /// </summary>
        public int HeartbeatStatus { get; set; }

        /// <summary>
        /// 更新包状态,详见EnumUpgradeBagStatus
        /// </summary>
        public int UpgradeBagStatus { get; set; }

        /// <summary>
        /// 更新状态详见EnumUpgradeStatus
        /// </summary>
        public int UpgradeStatus { get; set; }

        /// <summary>
        /// 是否单店
        /// </summary>
        public bool IsSingle { get; set; }
    }
}
