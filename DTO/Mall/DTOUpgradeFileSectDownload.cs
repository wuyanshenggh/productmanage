using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTOUpgradeFileSectDownload : BaseRequest
    {
        /// <summary>
        /// 版本ID
        /// </summary>
        public Guid VersionId { get; set; }

        /// <summary>
        /// 本次取的大小
        /// </summary>
        public int CurrSize { get; set; }

        /// <summary>
        /// 本次读取开始位置
        /// </summary>
        public long CurrPosition { get; set; }
    }

    public class DTOBackFileSection : ReturnResponse
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// 本次传输大小
        /// </summary>
        public int CurrSize { get; set; }

        /// <summary>
        /// 本次文件读取的位置
        /// </summary>
        public long CurrPosition { get; set; }

        /// <summary>
        /// 文件内容
        /// </summary>
        public byte[] FileBuffer { get; set; }
    }
}
