using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductMange.Model
{
    [Table("Prc_UserInfo")]
    public class Prc_UserInfo: Prc_BaseInfo
    {
       

     

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
        /// 权限
        /// </summary>
        [MaxLength(500)]
        public virtual string Rights
        {
            get;
            set;
        }
        /// <summary>
        ///  密码
        /// </summary>
        public virtual string GetRightName(string modeCode)
        {
            switch (modeCode)
            {
                case "1001":
                    return "版本管理";
                case "1002":
                    return "预约升级";
                case "1003":
                    return "用户管理";
                case "1004":
                    return "升级日历";
                case "1005":
                    return "操作日志";
                default :
                    return "未定义";

            }
        }


    }
}
