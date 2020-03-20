using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ProductMange.Model;
using ProductMange.Public;

namespace ProductMange.Controllers
{

    public class BaseControllerLogin : Controller
    {
        public BaseControllerLogin()
        {

        }

        //public UserSession GetSession()
        //{
        //    var str = ComHttpContext.Current.Session.GetString(ConstPara.SESSION_KEY);
        //    //var str = ControllerContext.HttpContext.Session.GetString(ConstPara.SESSION_KEY);
        //    if (string.IsNullOrEmpty(str))
        //    {
        //        return null;
        //    }
        //    var session = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSession>(str);
        //    return session;

        //}
    }


    }
