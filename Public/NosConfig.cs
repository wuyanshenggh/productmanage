using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Public
{
    public class NosConfig
    {
        private static string _accessKey;
        public static string AccessKey
        {
            get
            {
                if (string.IsNullOrEmpty(_accessKey))
                {
                    _accessKey = ConfigurationUtil.GetSection<AppSettings>("AppSettings").NosAccessKey;
                    if (string.IsNullOrWhiteSpace(_accessKey))
                    {
                        _accessKey = "94fb0402a65d4c5aafa51bbf48c55075";
                    }
                }
                return _accessKey;
            }
        }

        private static string _bucketName;
        public static string BucketName
        {
            get
            {
                if (string.IsNullOrEmpty(_bucketName))
                {
                    _bucketName = ConfigurationUtil.GetSection<AppSettings>("AppSettings").NosBucketName;
                    if (string.IsNullOrWhiteSpace(_bucketName))
                    {
                        _bucketName = "vip-prod";
                    }
                }
                return _bucketName;
            }
        }

        private static string _secretKey;
        public static string SecretKey
        {
            get
            {
                if (string.IsNullOrEmpty(_secretKey))
                {
                    _secretKey = ConfigurationUtil.GetSection<AppSettings>("AppSettings").NosSecretKey;
                    if (string.IsNullOrWhiteSpace(_secretKey))
                    {
                        _secretKey = "cd6fa5ba09c4433b8efc5391adb672be";
                    }
                }
                return _secretKey;
            }
        }

        private static string _endPoint;
        public static string EndPoint
        {
            get
            {
                if (string.IsNullOrEmpty(_endPoint))
                {
                    _endPoint = ConfigurationUtil.GetSection<AppSettings>("AppSettings").NosEndPoint;
                    if (string.IsNullOrWhiteSpace(_endPoint))
                    {
                        _endPoint = "nos-eastchina1.126.net";
                    }
                }
                return _endPoint;
            }
        }
    }
}
