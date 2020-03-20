using ProductMange.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.BLL.OneKeyUpgrade
{
    public class QuestionMsgFlag
    {
        public static Dictionary<string, List<EnumQuestionResult>> s_FlagToOperate = new Dictionary<string, List<EnumQuestionResult>>
        {
            {QuestionMsgFlag.UnZipFail,new List<EnumQuestionResult>{ EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },
            {QuestionMsgFlag.StartUpgradeExeFail,new List<EnumQuestionResult>{ EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },
            {QuestionMsgFlag.ExitServiceFail,new List<EnumQuestionResult>{ EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },
            {QuestionMsgFlag.StartServiceFail,new List<EnumQuestionResult>{ EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },
            {QuestionMsgFlag.BackupDBFail,new List<EnumQuestionResult>{ EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },
            {QuestionMsgFlag.BackupAppFail,new List<EnumQuestionResult>{ EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },
            {QuestionMsgFlag.DeleteOldBackupFail,new List<EnumQuestionResult>{ EnumQuestionResult.Ignore, EnumQuestionResult.ReTry, EnumQuestionResult.Stop } },

        };


        /// <summary>
        /// 解压失败
        /// </summary>
        public const string UnZipFail = "UN_ZIP_FAIL";

        /// <summary>
        /// 启动升级程序失败
        /// </summary>
        public const string StartUpgradeExeFail = "START_UPGRADE_EXE_FAIL";

        /// <summary>
        /// 关闭服务失败
        /// </summary>
        public const string ExitServiceFail = "EXIT_SERVICES_FAIL";

        /// <summary>
        /// 启动服务失败
        /// </summary>
        public const string StartServiceFail = "START_SERVICE_FAIL";

        /// <summary>
        /// 备份数据库失败
        /// </summary>
        public const string BackupDBFail = "BACKUP_DB_FAIL";

        /// <summary>
        /// 备份应用失败
        /// </summary>
        public const string BackupAppFail = "BACKUP_APP_FAIL";

        /// <summary>
        /// 删除旧备份文件失败
        /// </summary>
        public const string DeleteOldBackupFail = "DELETE_OLD_BACKUP_FAIL";
    }
}
