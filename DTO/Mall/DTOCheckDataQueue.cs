using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTOCheckDataQueue : BaseRequest
    {
        public Guid InfoId { get; set; }
    }

    public class DTOBackCheckDataQueue : ReturnResponse
    {
        /// <summary>
        /// 队列同步是否完成
        /// </summary>
        public bool Finished { get; set; }
    }
}
