using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    /// <summary>
    /// 最新版本信息
    /// </summary>
    public class DTOBackNewestVersionInfo : ReturnResponse
    {
        public Guid VersionID { get; set; }

        public string VersionNo { get; set; }
    }
}
