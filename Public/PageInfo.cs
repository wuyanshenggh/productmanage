using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class PageInfo
    {
        public int OffSet { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public string Sort { get; set; }
        public string SortOrder { get; set; }
        public bool IsAscending
        {
            get
            {
                if (SortOrder != "asc" && SortOrder != "desc")
                {
                    throw new CustomExecption("9999", $"{SortOrder}不正确");
                }
                bool isAscending=true;
                if (SortOrder == "desc")
                {
                    isAscending = false;
                }
                else if (SortOrder == "asc")
                {
                    isAscending = true;
                }

                return isAscending;
            }
        }
    }
}
