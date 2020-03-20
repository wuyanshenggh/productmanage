using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class CustomExecption : Exception
    {
        public string ErrorCode { get; set; }

        public CustomExecption(string errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

   
    }
}
