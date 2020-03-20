using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class DTOBackVersionInfo : ReturnResponse
    {
        public List<DTOVersionInfo> rows { get; set; }
        public int total { get; set; }
    }

    public class DTOVersionInfo
    {
        public Guid ID { get; set; }

        /// <summary>
        /// 是否已发布
        /// </summary>
        public virtual bool IsPublish { get; set; }

        /// <summary>
        ///  版本号，如10.1.50001
        /// </summary>
        public virtual string VersionNo
        {
            get;
            set;
        }

        /// <summary>
        ///  发布日期
        /// </summary>

        public virtual DateTime PublishDate
        {
            get;
            set;
        }

        /// <summary>
        /// 最后发布时间（可以多次发布）
        /// </summary>
        public virtual DateTime? LastPublishTime
        {
            get;set;
        }

        /// <summary>
        /// 更新包名称
        /// </summary>
        public string UpgradeBagName { get; set; }

        /// <summary>
        /// 更新内容
        /// </summary>
        public virtual string Context
        {
            get;
            set;
        }

        /// <summary>
        /// 操作用户
        /// </summary>
        public virtual string LastOperateUser
        {
            get;
            set;
        }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public virtual DateTime LastUpdateTime
        {
            get;
            set;
        }
    }
}
