using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class BasePageRequest
    {
        public int OffSet { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string SortOrder { get; set; }
    }
}
