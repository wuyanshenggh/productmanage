using ProductMange.Model.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model
{
    [Table("Prc_UpgradeMessage")]
    public class Prc_UpgradeMessage : Prc_BaseInfo
    {
        /// <summary>
        /// 所属更新细项
        /// </summary>
        public Prc_UpgradeInfoItem UpgradeInfoItem { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 处理结果，只对EnumMessageType.Question类有意义
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public EnumMessageType MessageType { get; set; }

        /// <summary>
        /// 消息标志
        /// </summary>
        public string MessageFlag { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public EnumHandleStatus HandleStatus { get; set; }

        /// <summary>
        /// 扩展信息
        /// </summary>
        public string ExtendInfo { get; set; }

        [NotMapped]
        public string BusinessNum { get; set; }
    }
}
