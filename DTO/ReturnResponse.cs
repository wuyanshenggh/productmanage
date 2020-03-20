using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class ReturnResponse
    {
        public ReturnResponse()
        {
            ResponseStatus = new ResponseStatus() { ErrorCode = "0" };
        }
        public ResponseStatus ResponseStatus { get; set; }
    }
}
