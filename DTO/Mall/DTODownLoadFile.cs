using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTODownLoadFile : BaseRequest
    {
        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string FileRelativePath { get; set; }

        /// <summary>
        /// 本次取的大小
        /// </summary>
        public int CurrSize { get; set; }

        /// <summary>
        /// 本次读取开始位置
        /// </summary>
        public long CurrPosition { get; set; }
    }
}
