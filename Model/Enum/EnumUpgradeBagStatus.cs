using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model.Enum
{
    /// <summary>
    /// 更新包状态
    /// </summary>
    public enum EnumUpgradeBagStatus
    {
        /// <summary>
        /// 未下发
        /// </summary>
        None = 1,
        /// <summary>
        /// 下发中
        /// </summary>
        Downloading = 2,
        /// <summary>
        /// 已下发
        /// </summary>
        Downloaded = 3,
        /// <summary>
        /// 下发异常
        /// </summary>
        Abnormal = 4
    }
}
