using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{


    public class DTOUserSession : ReturnResponse
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
 

 



    }
}
