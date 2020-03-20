using Microsoft.AspNetCore.Mvc.Filters;
using ProductMange.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProductMange.Public.OutInterface
{
    public class OutInterfaceActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.Controller is OutInterfaceBaseController)
            {
                if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.GetCustomAttributes(typeof(IAllowAnonymous), true).Length > 0)
                {
                    return;
                }
                if (context.Controller.GetType().GetCustomAttributes(typeof(IAllowAnonymous), true).Length > 0)
                {
                    return;
                }

                OutInterfaceValidate.Validate(context);
            }
        }
    }
}
