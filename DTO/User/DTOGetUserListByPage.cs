using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class DTOGetUserListByPage : BasePageRequest
    {
        public string LoginName { get; set; }
        public string UserName { get; set; }
    }
}
