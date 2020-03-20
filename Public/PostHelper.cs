using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductMange.Public
{
    public class PostHelper
    {
        /// <summary>
        /// 发送信息到平台/总部
        /// </summary>
        public static Rsp Post<Req, Rsp>(Req data, string url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            webRequest.Method = "POST";
            byte[] dataJson =
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = dataJson.Length;
            Stream newStream = webRequest.GetRequestStream();
            newStream.Write(dataJson, 0, dataJson.Length);
            newStream.Close();
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string resultJson = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<Rsp>(resultJson);
                }

            }
        }
    }
}
