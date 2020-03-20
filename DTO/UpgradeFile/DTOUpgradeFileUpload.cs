using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.UpgradeFile
{
    public class DTOUpgradeFileUpload : BaseRequest
    {
        /// <summary>
        /// 文件对应的版本ID
        /// </summary>
        public Guid VersionId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// 本次传输大小
        /// </summary>
        public int CurrSize { get; set; }

        /// <summary>
        /// 本次文件写入位置
        /// </summary>
        public int CurrPosition { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public List<byte> FileBuffer { get; set; }
    }
}
