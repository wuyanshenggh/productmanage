using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.DTO.Mall
{
    public class DTOGetVersionInfoById : BaseRequest
    {
        public Guid VersionId { get; set; }
    }

    public class DTOBackVersionInfoById : ReturnResponse
    {
        public Guid VersionID { get; set; }

        public string VersionNo { get; set; }

        public string UpgradeBagName { get; set; }
    }
}
