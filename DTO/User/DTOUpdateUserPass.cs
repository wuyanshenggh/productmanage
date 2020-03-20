using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{


    public class DTOUpdateUserPass : ReturnResponse
    { 

        /// <summary>
        ///  原密码
        /// </summary>
        public virtual string OldPass
        {
            get;
            set;
        }


        /// <summary>
        ///  新密码
        /// </summary>
        public virtual string NewPass
        {
            get;
            set;
        }
    }
}
