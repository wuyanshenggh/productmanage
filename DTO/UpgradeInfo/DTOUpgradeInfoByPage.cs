using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeInfo
{
    /// <summary>
    /// 查询预约升级信息
    /// </summary>
    public class DTOGetUpgradeInfoByPage : BasePageRequest
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string MallName { get; set; }

        /// <summary>
        /// 开始确认时间
        /// </summary>
        public string StartConfirmTime { get; set; }

        /// <summary>
        /// 结束确认时间
        /// </summary>
        public string EndConfirmTime { get; set; }

        /// <summary>
        /// 更新状态
        /// </summary>
        public int UpgradeStatus { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DTOBackUpgradeInfoByPage : ReturnResponse
    {
        public List<UpgradeInfo> rows { get; set; }
        public int total { get; set; }
    }

    public class UpgradeInfo
    {
        public Guid ID { get; set; }

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
        public string OriginalVersion { get; set; }

        /// <summary>
        /// 目标版本
        /// </summary>
        public string TargetVersion { get; set; }

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
        public int UpgradeStatus { get; set; }

        /// <summary>
        /// 确认更新时间
        /// </summary>
        public string ConfirmUpgradeTime { get; set; }

        /// <summary>
        /// 心跳状态(-1非升级时间,0正常，1异常)
        /// </summary>
        public int HeartbeatStatus { get; set; }

        /// <summary>
        /// 升级耗时
        /// </summary>
        public int TimeConsum { get; set; }

        /// <summary>
        /// 异常数量
        /// </summary>
        public int AbnormalCount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Summary { get; set; }
    }
}
