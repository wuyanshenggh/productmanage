using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMange.Model;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
namespace ProductMange.Controllers
{
    // [Route("api/Product/Login")]
    [Produces("application/json")]
    public class LoginController: Controller
    {


        public ProductManageDbContext DbContext { get; }
        public LoginController(ProductManageDbContext _DbContext)
        {            
            DbContext = _DbContext;
        }


        [HttpPost]
        [Route("api/Product/Login/DTOLogin")]
        public ReturnResponse UserLogin(DTOLogin dto)
        {
            ReturnResponse res = new ReturnResponse();
            if (string.IsNullOrEmpty(dto.Uid.Trim())) throw new CustomExecption("1001", "用户名不能为空");
            bool IsPass = true;//[FromBody]
                               //string loginInfo = ConfigurationUtil.GetSection<AppSettings>("AppSettings").LoginInfo;// Environment.GetEnvironmentVariable("LoginInfo");
                               //if (string.IsNullOrEmpty(loginInfo)) IsPass = false;
                               //var lInfos = loginInfo.Split(":");
                               //if (lInfos.Count() != 2) IsPass = false;       
            var bll = new Repository<Prc_UserInfo>(DbContext);
            var user = bll.Get(o=>o.LoginName== dto.Uid.Trim()&& o.IsDelete == false);
            if (user==null) throw new CustomExecption("1001", "用户名不存在");
            var pass = DecodeBase64("utf-8", dto.Pid);
            if (user.PassWord != pass || dto.Uid.Trim() != user.LoginName)
            {
                IsPass = false;
            }
            if (!IsPass) throw new CustomExecption("1002", "用户名或者密码错误");
           // BaseControllerLogin sess = new BaseControllerLogin();
           // HttpContext.Session.SetString("sessionkey", "sessionvalue123");
            SaveSession(user);     
            return res;
        }

         void SaveSession(Prc_UserInfo model)
        {
            var session = new UserSession();
            session.ID = model.ID;
            session.LoginName = model.LoginName;
            session.UserName = model.UserName;
            session.Rights = model.Rights;
            //var sessions = this.HttpContext.Session;
            HttpContext.Session.SetString(ConstPara.SESSION_KEY, Newtonsoft.Json.JsonConvert.SerializeObject(session));

        }

        ///解码
        public static string DecodeBase64(string code_type, string code)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(code);
            try
            {
                decode = System.Text.Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = code;
            }
            return decode;
        }

    }
}
