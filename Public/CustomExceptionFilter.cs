using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using ProductMange.Controllers;

namespace ProductMange
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            Loger log = new Loger();
            // Console.WriteLine($"{context.Exception} 未处理的异常");
            if (((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor).ControllerTypeInfo.BaseType.IsAssignableFrom(typeof(OutInterfaceBaseController)))
            {
                log.Error(context.Exception.Message, context.Exception);
                context.Result = new ObjectResult(new ReturnResponse()
                {
                    ResponseStatus = new ResponseStatus()
                    {
                        ErrorCode = "500",
                        Message = context.Exception.Message
                    }
                });
            }
            else
            {
                if (context.Exception is CustomExecption)
                {
                    CustomExecption ex = context.Exception as CustomExecption;
                    var returnObj = new ReturnResponse()
                    {
                        ResponseStatus = new ResponseStatus()
                        {
                            ErrorCode = ex.ErrorCode.ToString(),
                            Message = ex.Message
                        }
                    };
                    context.Result = new ObjectResult(returnObj);
                }
                else
                {

                    //try
                    //{
                    //    ReturnResponse bussinessException = JsonConvert.DeserializeObject<ReturnResponse>(context.Exception.Message);
                    //    if (bussinessException != null && bussinessException.ResponseStatus.ErrorCode != "0")
                    //    {
                    //        context.HttpContext.Response.StatusCode = 200;
                    //        context.Result = new ObjectResult(new ReturnResponse()
                    //        {
                    //            ResponseStatus = new ResponseStatus()
                    //            {
                    //                ErrorCode = bussinessException.ResponseStatus.ErrorCode,
                    //                Message = bussinessException.ResponseStatus.Message
                    //            }
                    //        });
                    //        log.Error("200类错误", context.Exception);
                    //        return;
                    //    }
                    //}
                    //catch
                    //{
                    //    //不用处理
                    //}

                    context.HttpContext.Response.StatusCode = 500;
                    string detailstr = context.Exception.StackTrace;

                    log.Error(context.Exception.Message, context.Exception);

                    context.Result = new ObjectResult(new ReturnResponse()
                    {
                        ResponseStatus = new ResponseStatus()
                        {
                            ErrorCode = "500",
                            Message = context.Exception.Message
                        }
                    });

                }
            }
        }
    }
}
