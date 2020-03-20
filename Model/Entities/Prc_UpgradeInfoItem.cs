using ProductMange.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model
{
    /// <summary>
    /// 升级信息细项
    /// </summary>
    [Table("Prc_UpgradeInfoItem")]
    public class Prc_UpgradeInfoItem : Prc_BaseInfo
    {
        /// <summary>
        /// 所属升级详情
        /// </summary>
        [ForeignKey("UpgradeInfoID")]
        public virtual Prc_UpgradeInfo UpgradeInfo { get; set; }

        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 商户类型
        /// </summary>
        public EnumBusinessType BusinessType { get; set; }

        /// <summary>
        /// 更新包下发状态
        /// </summary>
        public EnumUpgradeBagStatus UpgradeBagStatus { get; set; }

        /// <summary>
        /// 更新状态
        /// </summary>
        public EnumUpgradeStatus UpgradeStatus { get; set; }

        /// <summary>
        /// 升级开始时间
        /// </summary>
        public DateTime? StartUpgradeTime { get; set; }

        /// <summary>
        /// 升级结束时间
        /// </summary>
        public DateTime? EndUpgradeTime { get; set; }

        /// <summary>
        /// 心跳状态(0正常，1异常，-1无)
        /// </summary>
        public int HeartbeatStatus { get; set; }

        /// <summary>
        /// 门店编码
        /// </summary>
        public string BusinessNum { get; set; }
    }
}
