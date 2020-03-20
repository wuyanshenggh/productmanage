using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    /// <summary>
    /// 预约升级
    /// </summary>
    public class DTOReserveUpgrade : BaseRequestMall
    {
        /// <summary>
        /// 是否单店
        /// </summary>
        public bool IsSingle { get; set; }
        /// <summary>
        /// 商城编码
        /// </summary>
        public string MallCode { get; set; }

        /// <summary>
        /// 商城名称
        /// </summary>
        public string MallName { get; set; }

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
        /// 申请时间
        /// </summary>
        public virtual DateTime ApplyTime { get; set; }

        /// <summary>
        /// 门店信息子集
        /// </summary>
        public List<BusinessInfo> BusinessInfos { get; set; }
    }

    /// <summary>
    /// 门店信息
    /// </summary>
    public class BusinessInfo
    {
        /// <summary>
        /// 商户名称
        /// </summary>
        public string BusinessName { get; set; }

        /// <summary>
        /// 商户类型
        /// </summary>
        public int BusinessType { get; set; }

        /// <summary>
        /// 门店编码
        /// </summary>
        public string BusinessNum { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class DTOBackReserveUpgrade : ReturnResponse
    {
        /// <summary>
        /// 升级信息ID
        /// </summary>
        public Guid InfoID { get; set; }
    }
}
