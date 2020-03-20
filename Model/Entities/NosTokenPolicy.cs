using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Model
{
    public class NosTokenPolicy
    {
        public string Bucket { get; set; }

        public string Object { get; set; }

        public long Expires { get; set; }
    }
}
