using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProductMange.Public
{
    public class PublicFun
    {
        //}
        public static string GenerateSign(Dictionary<string, string> param, string secretkeyName = "secretkey")
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
                sb.Append(vsecret); //将secretkey加到字符串前后
                foreach (string paramname in dict.Keys)
                {
                    if (paramname == secretkeyName.ToLower()) continue; //不要将secretkey当成参数
                    sb.Append(paramname);
                    sb.Append(dict[paramname] ?? "");
                }
                sb.Append(vsecret); //将secretkey加到字符串前后
                // 3.用MD5算法生成签名
                string signResult = GetMd5(sb.ToString(), "UTF-8").ToUpper();
                return signResult;
            }
            catch (Exception ex)
            {
                new Loger("ChannelValidation").Error("生成SIGN错误", ex);
                return "";
            }
        }

        /// <summary>
        /// 签名算法
        ///<para>
        ///签名生成的通用步骤如下：
        ///第一步，设所有发送或者接收到的数据为集合M，将集合M内非空参数值的参数按照参数名ASCII码从小到大排序（字典序），使用URL键值对的格式（即key1=value1&key2=value2…）拼接成字符串stringA。
        ///特别注意以下重要规则：
        ///◆ 参数名ASCII码从小到大排序（字典序）；
        ///◆ 如果参数的值为空不参与签名；
        ///◆ 参数名区分大小写；
        ///◆ 验证调用返回或微信主动通知签名时，传送的sign参数不参与签名，将生成的签名与该sign值作校验。
        ///◆ 微信接口可能增加字段，验证签名时必须支持增加的扩展字段
        ///第二步，在stringA最后拼接上key得到stringSignTemp字符串，并对stringSignTemp进行MD5运算，再将得到的字符串所有字符转换为大写，得到sign值signValue。
        ///</para>
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="secretkey"></param>
        /// <param name="secretkeyName"></param>
        /// <returns></returns>
        public static string GenerateSign2(object obj, string secretkey, string secretkeyName = "sign")
        {
            Dictionary<string, string> param = GetDictionary(obj);
            param = AscByAsciiKey(param);
            return GenerateSign2(param, secretkey, secretkeyName);
        }

        public static string GenerateSign2(Dictionary<string, string> param, string secretkey, string secretkeyName = "sign")
        {
            try
            {
                if (string.IsNullOrEmpty(secretkey)) return "";

                StringBuilder sb = new StringBuilder();
                foreach (string paramname in param.Keys)
                {
                    if (paramname.ToLower() == secretkeyName.ToLower()) continue; //不要将secretkey当成参数
                    if (param[paramname] == null) continue;

                    if (sb.Length != 0) sb.Append("&");

                    sb.Append(paramname);
                    sb.Append("=");
                    sb.Append(param[paramname]);
                }
                sb.Append(secretkeyName); //将secretkey加到字符串前后
                sb.Append("=");
                sb.Append(secretkey);

                // 3.用MD5算法生成签名
                string signResult = GetMd5(sb.ToString(), "UTF-8").ToUpper();
                return signResult;
            }
            catch (Exception ex)
            {
                new Loger("SignValidation").Error("生成SIGN错误", ex);
                return "";
            }
        }

        /// <summary>
        /// 反射对象的属性和值
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetDictionary(object o)
        {
            var sortedParams = new Dictionary<string, string>();
            if (o == null)
                return sortedParams;
            var type = o.GetType();
            foreach (var p in type.GetProperties())
            {
                var name = p.Name;
                var value = p.GetValue(o, null);
                if (p.PropertyType.IsValueType || p.PropertyType.Name.StartsWith("String"))
                {
                    if (value == null)
                    {
                        continue;
                    }
                    else
                    {
                        sortedParams.Add(name, value.ToString());
                    }
                }
                else if (p.PropertyType.IsGenericType || p.PropertyType.IsClass)
                {
                    sortedParams.Add(name, value == null ? "" : JsonConvert.SerializeObject(value));
                }
            }
            return sortedParams;
        }

        public static Dictionary<string, string> AscByAsciiKey(Dictionary<string, string> data)
        {
            Dictionary<string, string> tmp = new Dictionary<string, string>();
            var keys = new List<string>(data.Keys.ToArray());
            keys.Sort((o, t) =>
            {
                if (o == t) return 0;

                var obytes = Encoding.ASCII.GetBytes(o);
                var tbytes = Encoding.ASCII.GetBytes(t);

                var len = obytes.Length > tbytes.Length ? obytes.Length : tbytes.Length;
                int value = 0;
                for (int j = 0; j < len; j++)
                {
                    if (obytes.Length <= j)
                    {
                        return -1;
                    }
                    if (tbytes.Length <= j)
                    {
                        return 1;
                    }
                    value = obytes[j] - tbytes[j];
                    if (value == 0) continue;
                    break;
                }

                return value;
            });

            foreach (var key in keys)
            {
                tmp.Add(key, data[key]);
            }

            return tmp;
        }

        public static string GetMd5(string dataStr, string codeType)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            var bytes = md5.ComputeHash(Encoding.GetEncoding(codeType).GetBytes(dataStr));
            var sb = new StringBuilder(32);
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }
        public static byte[] SerializeObject(object obj)
        {
            if (obj == null)
                return null;
            var ms = new MemoryStream();
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;
            var bytes = ms.GetBuffer();
            ms.Read(bytes, 0, bytes.Length);
            ms.Close();
            return bytes;
        }

        /// <summary>  
        /// 把字节数组反序列化成对象  
        /// </summary>  
        public static object DeserializeObject(byte[] bytes)
        {
            if (bytes == null)
                return null;
            var ms = new MemoryStream(bytes) { Position = 0 };
            var formatter = new BinaryFormatter();
            var obj = formatter.Deserialize(ms);
            ms.Close();
            return obj;
        }

        /// <summary>
        /// 验签加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string CreatSecretkey(string data)
        {
            //拼接签名数据
            //将字符串中字符按升序排序
            var sortStr = string.Concat(data.OrderBy(c => c));
            var secretkey = GetMd5(sortStr, "UTF-8").ToUpper();
            return secretkey;

        }
    }
}
