using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class AppSettings
    {
        public string ConnStr { get; set; }
        public string LoginInfo { get; set; }

        public string NosBucketName { get; set; }

        public string NosAccessKey { get; set; }

        public string NosSecretKey { get; set; }

        public string NosEndPoint { get; set; }
    }
}
