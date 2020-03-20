using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using ProductMange.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using ProductMange.Public;

namespace ProductMange
{
    public class CustomActionFilter : ActionFilterAttribute
    {
       // readonly IServiceCollection services;
        public string ModelCode { set; get; }
        public CustomActionFilter()//IServiceCollection services
        {
          //  this.services = services;
        }
        public  override void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is BaseControllerLogin)
            {
                string str = context.HttpContext.Session.GetString(ConstPara.SESSION_KEY);
                if (string.IsNullOrEmpty(str))
                {
                    throw new CustomExecption("401", "未登录");
                }

                if (!string.IsNullOrEmpty(str))
                {
                    UserSession userSession = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSession>(str);
                    if (ModelCode != null && !userSession.Rights.Contains(ModelCode))
                    {
                        throw new CustomExecption("417", "没有权限");
                    }
                }
            }
        }
    }

}
