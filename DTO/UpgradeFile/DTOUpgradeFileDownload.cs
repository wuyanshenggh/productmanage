using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeFile
{
    public class DTOUpgradeFileDownload : BaseRequest
    {
        /// <summary>
        /// 文件对应的版本ID
        /// </summary>
        public Guid VersionId { get; set; }
    }
}
