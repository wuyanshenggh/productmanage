using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class DTOBackVersionDetail:ReturnResponse
    {
        public Guid ID { get; set; }
        public string VersionNo { get; set; }
        public DateTime PublishDate { get; set; }
        public string Content { get; set; }
    }
}
