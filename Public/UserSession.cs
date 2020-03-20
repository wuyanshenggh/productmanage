using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace ProductMange
{
    [JsonObject]
    public class UserSession
    {
        public Guid ID { get; set; }


        public string LoginName { get; set; }

        public string Rights { get; set; }

        public string UserName { get; set; }
         
    }
}
