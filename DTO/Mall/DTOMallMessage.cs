using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTOMallMessage : BaseRequestMall
    {
        public Guid PfInfoId { get; set; }
        /// <summary>
        /// 消息集合
        /// </summary>
        public List<MallMessage> Messages { get; set; }
    }

    /// <summary>
    /// 娱乐管家消息内容
    /// </summary>
    public class MallMessage
    {
        /// <summary>
        /// 消息ID
        /// </summary>
        public Guid ID { get; set; }
        /// <summary>
        /// 门店编号
        /// </summary>
        public string BusinessNum { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime OccurTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }

        /// <summary>
        /// 消息标识
        /// </summary>
        public string MessageFlag { get; set; }
    }

    public class DTOBackMallMessage : ReturnResponse
    {
        /// <summary>
        /// 写入失败的消息
        /// </summary>
        public Dictionary<Guid, string> FailMessages { get; set; }
    }
}
