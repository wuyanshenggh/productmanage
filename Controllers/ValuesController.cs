using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductMange.Model;

namespace ProductMange.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {

        public ProductManageDbContext DbContext { get; }


        public ValuesController(ProductManageDbContext _DbContext)
        {
            DbContext = _DbContext;
        }



        [HttpPost]
        public IActionResult SyncVersionInfo(DateTime syncTime)
        {
            var versionInfo = DbContext.Prc_VersionInfos.Where(o => o.IsPublish && o.LastPublishTime > syncTime).OrderBy(o => o.LastPublishTime).Skip(0).Take(1).FirstOrDefault();
            if (versionInfo != null)
            {
                return Json(versionInfo);
            }

            return Json(null);
        }
    }
}

