using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ProductMange.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Public.OutInterface
{
    /// <summary>
    /// 外部接口校验
    /// </summary>
    public class OutInterfaceValidate
    {
        static string Secretkey = "OKUD9DC93EB71AE54FF5A3C8AA6901392F9C";
        public static void Validate(ActionExecutingContext context)
        {
           string str =  context.ActionDescriptor.DisplayName;
            var now = DateTime.UtcNow;
            string Timestamp, TPKey, reqSign = "";
            Guid BusinessID = Guid.Empty, MallID = Guid.Empty;
            TPKey = context.HttpContext.Request.Headers["TPKey"];
            Timestamp = context.HttpContext.Request.Headers["Timestamp"];
            reqSign = context.HttpContext.Request.Headers["Sign"];
            
            string sign = "";
            try
            {
                string bodyStr = GetBody(context.HttpContext);
                sign = OutInterfaceAPISign.CreateSign(bodyStr, TPKey, Timestamp, Secretkey);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (reqSign != sign)
                throw new Exception("验签失败");
            TimeSpan ts = now - DateTime.Parse(Timestamp);
            if (Math.Abs(ts.TotalMinutes) > 10 || Math.Abs(ts.TotalMinutes) < -10)
            {
                throw new Exception("请求过时");
            }
        }

        static string GetBody(HttpContext context)
        {
            context.Request.EnableRewind();
            context.Request.Body.Position = 0;
            //context.Request.Body.CopyTo(stream)
            byte[] bytes = new byte[(int)context.Request.ContentLength];
            context.Request.Body.Read(bytes, 0, (int)context.Request.ContentLength);
            string body = "";
            using (Stream stream = new MemoryStream(bytes))
            {
                body = System.Text.Encoding.UTF8.GetString(bytes);
            }
            return body;
        }
    }
}
