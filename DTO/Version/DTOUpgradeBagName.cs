using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Version
{
    /// <summary>
    /// 升级包名称
    /// </summary>
    public class DTOUpgradeName : BaseRequest
    {
        /// <summary>
        /// 更新包名称
        /// </summary>
        public string BagName { get; set; }

        /// <summary>
        /// 版本ID
        /// </summary>
        public Guid VersionId { get; set; }
    }
}
