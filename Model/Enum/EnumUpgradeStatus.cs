using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model.Enum
{
    /// <summary>
    /// 更新状态
    /// </summary>
    public enum EnumUpgradeStatus
    {
        /// <summary>
        /// 已预约
        /// </summary>
        Reserved = 1,
        /// <summary>
        /// 升级中
        /// </summary>
        Upgrading = 2,
        /// <summary>
        /// 升级成功
        /// </summary>
        UpgradeSucc = 3,
        /// <summary>
        /// 升级失败
        /// </summary>
        UpgradeFail = 4,
        /// <summary>
        /// 已过期
        /// </summary>
        Overdued = 5,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 6
    }
}
