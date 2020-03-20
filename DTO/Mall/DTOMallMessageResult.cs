using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTOGetMallMessageResult: BaseRequestMall
    {
        /// <summary>
        /// Message表的ID
        /// </summary>
        public List<Guid> MessageIds { get; set; }
    }

    public class DTOBackMallMessageResult : ReturnResponse
    {
        /// <summary>
        /// 结果集
        /// </summary>
        public Dictionary<Guid, string> Results { get; set; }
    }
}
