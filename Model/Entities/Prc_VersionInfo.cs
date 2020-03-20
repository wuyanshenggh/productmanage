using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductMange.Model
{
    [Table("Prc_VersionInfo")]
    public class Prc_VersionInfo: Prc_BaseInfo
    {
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
        /// 更新内容
        /// </summary>
        //[NHProperty(Length = 2000000)]
        public virtual byte[] Context
        {
            get;
            set;
        }


        /// <summary>
        /// 最后发布时间
        /// </summary>
        public virtual DateTime? LastPublishTime
        {
            get;
            set;
        }

        /// <summary>
        /// 更新包名称
        /// </summary>
        public string UpgradeBagName { get; set; }

    }
}
