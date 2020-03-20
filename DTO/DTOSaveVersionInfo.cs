using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange
{
    public class DTOSaveVersionInfo:ReturnResponse
    {
        public string ID { get; set; }
        public string VersionNo { get; set; }
        public string PublishDate { get; set; }
        public string Content { get; set; }
    }
}
