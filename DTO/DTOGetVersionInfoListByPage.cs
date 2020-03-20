using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class DTOGetVersionInfoListByPage:BasePageRequest
    {
        public string VersionNo { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
