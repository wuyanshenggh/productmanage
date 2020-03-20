using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Public
{
    public class LoginUserInfo
    {
        public static UserSession CurrUser
        {
            get
            {
                if (ComHttpContext.Current != null)
                {
                    var str = ComHttpContext.Current.Session.GetString(ConstPara.SESSION_KEY);
                    if (string.IsNullOrEmpty(str))
                    {
                        return null;
                    }
                    var session = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSession>(str);
                    return session;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
