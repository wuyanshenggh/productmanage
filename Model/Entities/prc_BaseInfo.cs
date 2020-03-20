using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductMange.Model
{
 
    public class Prc_BaseInfo
    {
        [Key]
        public virtual Guid ID { get; set; }

        /// <summary>
        ///  创建时间
        /// </summary>
        public virtual DateTime CreateTime
        {
            get;
            set;
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        public virtual string CreateOperateUser
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

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDelete { get; set; }
    }
}
