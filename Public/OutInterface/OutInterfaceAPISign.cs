using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProductMange.Public.OutInterface
{
    public class OutInterfaceAPISign
    {
        static Type dateTimeType = typeof(DateTime);
        static Type guidType = typeof(Guid);

        public static string CreateSign(string body, string TPKey, string Timestamp, string secretkey)
        {

            Dictionary<string, string> param = new Dictionary<string, string>();
            JObject jObject = JObject.Parse(body);
            param = GetDict(jObject);
            param.Add("TPKey", TPKey);
            param.Add("Timestamp", Timestamp);
            param.Add("secretkey", secretkey);
            string sign = GenerateSign(param);
            return sign;
        }

        /// <summary>
        /// 创建签名
        /// </summary>
        /// <param name="obj">DTO对象</param>
        /// <param name="timestamp">时间字符串</param>
        /// <param name="tpKey">签名KEY</param>
        /// <param name="secretkey">签名密码</param>
        /// <returns></returns>
        public static string CreateSign(object obj, string TPKey, string Timestamp, string secretkey)
        {

            Dictionary<string, string> param = new Dictionary<string, string>();
            if (obj != null)
            {
                string postData = JsonConvert.SerializeObject(obj);
                var jsonObj = JObject.Parse(postData);
                param = GetDict(jsonObj);
            }
            param.Add("TPKey", TPKey);
            param.Add("Timestamp", Timestamp);
            param.Add("secretkey", secretkey);
            string sign = GenerateSign(param);
            return sign;
        }
        static Dictionary<string, string> GetDict(JObject jsonobj, string prex = null)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            foreach (var js in jsonobj)
            {

                if (js.Value.Type == JTokenType.Array)
                {

                    for (int i = 0; i < js.Value.Count(); i++)
                    {
                        var k = js.Value[i];
                        string keys = GetKey(prex, $"{js.Key}_{i}");
                        if (k is JValue)
                        {
                            param.Add(keys, k.ToString());
                        }
                        else
                        {
                            var dict = GetDict((JObject)k, keys);
                            MergeDic(param, dict);
                        }
                    }
                }
                else if (js.Value.Type == JTokenType.Object)
                {
                    var dict = GetDict((JObject)js.Value, GetKey(prex, js.Key));
                    MergeDic(param, dict);
                }
                else
                {
                    param.Add(GetKey(prex, js.Key), js.Value.ToString());
                }

            }
            return param;
        }

        static void MergeDic(Dictionary<string, string> source, Dictionary<string, string> data)
        {
            foreach (var kv in data)
            {
                source.Add(kv.Key, kv.Value);
            }
        }

        private static string GetKey(string prex, string key)
        {
            return ((string.IsNullOrEmpty(prex) ? "" : (prex + "_")) + key).ToLower();
        }

        static string GenerateSign(Dictionary<string, string> param, string secretkeyName = "secretkey")
        {
            try
            {
                if (param == null || secretkeyName == null) return "";

                //排序字典
                SortedDictionary<string, string> dict = new SortedDictionary<string, string>();
                // 1.参数KEY转换为小写
                foreach (string key in param.Keys)
                {
                    string k = key.ToLower();
                    if (k == "sign") continue; //删除sign参数
                    dict.Add(k, param[key]);
                }

                // 2.将参数名和参数值组成字符串，将secretkey加到字符串前后
                string vsecret = dict[secretkeyName.ToLower()];
                StringBuilder sb = new StringBuilder();
                // sb.Append(vsecret); //将secretkey加到字符串前后
                foreach (string paramname in dict.Keys)
                {
                    if (paramname == secretkeyName.ToLower()) continue; //不要将secretkey当成参数
                    sb.Append(paramname);

                    sb.Append((dict[paramname] ?? "").ToLower());
                }
                sb.Append(vsecret); //将secretkey加到字符串前后
                                    // 3.用MD5算法生成签名
                string signResult = GetMD5(sb.ToString(), "UTF-8").ToUpper();
#if DEBUG
                //loger.Info("加密数据：{0}  加密：{1}".Fmt(sb.ToString(), signResult));
#endif
                return signResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成SIGN错误", ex);
                return "";
            }
        }

        static string GetMD5(string dataStr, string codeType)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(codeType).GetBytes(dataStr));
            System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
    }
}
