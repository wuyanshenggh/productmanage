using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Version
{
    /// <summary>
    /// 获取上传信息
    /// </summary>
    public class DTOGetNosUploadInfo : BaseRequest
    {
        public string ObjectName { get; set; }
    }

    public class DTOBackNosUploadInfo : ReturnResponse
    {
        /// <summary>
        /// 桶名
        /// </summary>
        public string BucketName { get; set; }

        /// <summary>
        /// 上传凭证
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        public string EndPoint { get; set; }
    }
}
