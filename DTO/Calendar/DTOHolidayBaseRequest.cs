
using Newtonsoft.Json;
using ProductMange.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProductMange.DTO
{
    /// <summary>
    /// 节假日请求类
    /// </summary>
    public class DTOHolidayBaseRequest
    {
        //验证串
        public string Sign { get; set; }
        /// <summary>
        /// 时间戳(10分钟)
        /// </summary>
        public long TS { get; set; }
        /// <summary>
        /// 娱乐管家门店编号
        /// </summary>
        public string BusinessId { get; set; }
        /// <summary>
        /// 娱乐管家渠道类别
        /// </summary>
        public string BusinessName { get; set; }


        public T SetSign<T>(T o) where T : DTOHolidayBaseRequest
        {
            //当前时间戳
            long currenttimemillis = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

            o.TS = currenttimemillis;
            var secretkey = PublicFun.CreatSecretkey(o.BusinessId);
            var param = PublicFun.GetDictionary(o);
            param.Add("secretkey", secretkey);
            var signs = PublicFun.GenerateSign(param);
            o.Sign = signs;
            return o;
        }
    }
}
