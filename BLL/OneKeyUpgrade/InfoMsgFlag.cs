using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.BLL.OneKeyUpgrade
{
    public class InfoMsgFlag
    {
        /// <summary>
        /// 开始下载更新包
        /// </summary>
        public const string StartDownloadUpgradeBag = "START_DOWNLOAD_UPGRADE_BAG";

        /// <summary>
        /// 完成下载
        /// </summary>
        public const string FinishDownloadUpgradeBag = "FINISH_DOWNLOAD_UPGRADE_BAG";

        /// <summary>
        /// 开始升级
        /// </summary>
        public const string StartUpgrade = "START_UPGRADE";

        /// <summary>
        /// 服务关闭成功
        /// </summary>
        public const string ExitServicesSucc = "EXIT_SERVICES_SUCC";

        /// <summary>
        /// 升级成功
        /// </summary>
        public const string UpgradeSucc = "UPGRADE_SUCC";

        /// <summary>
        /// 数据同步中
        /// </summary>
        public const string DataSynTaskWait = "DATA_SYN_TASK_WAIT";

        /// <summary>
        /// 数据同步任务完成
        /// </summary>
        public const string DataSynTaskFinish = "DATA_SYN_TASK_FINISH";
    }
}
