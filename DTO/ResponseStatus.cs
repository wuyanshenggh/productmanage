using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class ResponseStatus
    {

       // [JsonProperty("ErrorCode")]
        public string ErrorCode { get; set; }
      //  [JsonProperty("Message")]
        public string Message { get; set; }
    }
}
