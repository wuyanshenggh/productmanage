using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    /// <summary>
    /// 预约升级
    /// </summary>
    public class DTOHeartbeat : BaseRequestMall
    {
        /// <summary>
        /// 关联ID
        /// </summary>
        public Guid InfoID { get; set; }

        /// <summary>
        /// 网络异常的门店编码
        /// </summary>
        public List<string> LostNetworkBusNum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DTOBackHeartbeat : ReturnResponse
    {
        /// <summary>
        /// 确认升级时间
        /// </summary>
        public DateTime? ConfirmUpgradeTime { get; set; }

        /// <summary>
        /// 是否取消
        /// </summary>
        public bool IsCancel { get; set; }

        /// <summary>
        /// 是否开始升级
        /// </summary>
        public bool IsStartUpgrade { get; set; }

        /// <summary>
        /// 各门店的更新状态
        /// </summary>
        public List<string> CancelBusiness { get; set; }
    }
}
