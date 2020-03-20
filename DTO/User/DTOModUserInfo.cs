using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{


    public class DTOModUserInfo : ReturnResponse
    {
        public Guid ID { get; set; }


        /// <summary>
        ///  登陆名
        /// </summary>
        public virtual string LoginName
        {
            get;
            set;
        }


        /// <summary>
        ///  姓名
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }

        /// <summary>
        ///  密码
        /// </summary>
        public virtual string PassWord
        {
            get;
            set;
        }

        /// <summary>
        ///  权限
        /// </summary>
        public virtual string Rights
        {
            get;
            set;
        }




    }
}
