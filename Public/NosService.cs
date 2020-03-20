using Netease.Cloud.NOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Public
{
    public class NosService
    {
        public static void DeleteFile(string fileName, string BucketName = null)
        {
            try
            {
                var nosClient = new NosClient(NosConfig.EndPoint, NosConfig.AccessKey, NosConfig.SecretKey);
                bool exist = nosClient.DoesObjectExist(NosConfig.BucketName, fileName);
                if (exist)
                {
                    nosClient.DeleteObject(NosConfig.BucketName, fileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
