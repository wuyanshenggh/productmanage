using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.BLL.OneKeyUpgrade
{
    public class ErrorMsgFlag
    {
        /// <summary>
        /// 因升级失败停止升级
        /// </summary>
        public const string UpgradeFail = "UPGRADE_FAIL";

        /// <summary>
        /// 因取消停止升级
        /// </summary>
        public const string UpgradeCancel = "UPGRADE_CANCEL";

        /// <summary>
        /// 因过期停止升级
        /// </summary>
        public const string UpgradeOverdued = "UPGRADE_OVERDUED";
    }
}
