using ProductMange.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model
{
    [Table("Prc_UpgradeInfo")]
    public class Prc_UpgradeInfo : Prc_BaseInfo
    {
        /// <summary>
        /// 是否单店
        /// </summary>
        public bool IsSingle { get; set; }
        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }
        /// <summary>
        /// 商城编号
        /// </summary>
        public string MallCode { get; set; }
        /// <summary>
        /// 原版本
        /// </summary>
        public string OriginalVersionNo { get; set; }

        /// <summary>
        /// 目标版本
        /// </summary>
        public string TargetVersionNo { get; set; }

        /// <summary>
        /// 目标版本ID
        /// </summary>
        public Guid TargetVersionID { get; set; }

        /// <summary>
        /// 门店数量
        /// </summary>
        public int BusinessCount { get; set; }
       
        /// <summary>
        /// 预约时间
        /// </summary>
        public DateTime ReserveTime { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 更新状态
        /// </summary>
        public EnumUpgradeStatus UpgradeStatus { get; set; }

        /// <summary>
        /// 确认更新时间
        /// </summary>
        public DateTime? ConfirmUpgradeTime { get; set; }

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplyTime { get; set; }

        /// <summary>
        /// 上一次收到心跳的时间
        /// </summary>
        public DateTime HeartbeatTime { get; set; }

        /// <summary>
        /// 开始更新时间
        /// </summary>
        public DateTime? StartUpgradeTime { get; set; }

        /// <summary>
        /// 结束更新时间
        /// </summary>
        public DateTime? EndUpgradeTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Summary { get; set; }
    }
}
