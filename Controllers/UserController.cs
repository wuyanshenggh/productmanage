using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMange.Model;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ProductMange.Controllers
{

    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {

        public ProductManageDbContext DbContext { get; }


        public UserController(ProductManageDbContext _DbContext)
        {
            DbContext = _DbContext;
        }


        [HttpPost]
        public IActionResult GetCurUser(BasePageRequest dto)
        {
           var str = HttpContext.Session.GetString(ConstPara.SESSION_KEY);
            //if (!string.IsNullOrEmpty(str))
            //{

            //    throw new CustomExecption("401", "未登录");
            //}
            var uInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSession>(str);
            var userdto = new DTOUserSession();
            userdto.ID = uInfo.ID;
            userdto.LoginName = uInfo.LoginName;
            userdto.UserName = uInfo.UserName;
            return new JsonResult(userdto);
        }
        [HttpPost]
        [CustomActionFilter(ModelCode = "1003")]
        public IActionResult Search([FromBody]DTOGetUserListByPage dto)
        {
            DTOUserList result = new DTOUserList();
            result.rows = new List<DTOUserInfo>();
            PageInfo pageInfo = new PageInfo { OffSet=dto.OffSet,PageSize=dto.PageSize,Sort=dto.Sort,SortOrder=dto.SortOrder};
            List<Expression<Func<Prc_UserInfo, bool>>> whereConditions = new List<Expression<Func<Prc_UserInfo, bool>>>();
            if (!string.IsNullOrWhiteSpace(dto.LoginName.Trim()))
            {
                whereConditions.Add(o => o.LoginName.Contains(dto.LoginName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(dto.UserName.Trim()))
            {
                whereConditions.Add(o => o.UserName.Contains(dto.UserName.Trim()));
            }
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;            
            whereConditions.Add(o => !o.IsDelete);
           //using (var unitOfWork = new UnitOfWork(new DataAccess()))
            //{
            //    var list = unitOfWork.VersionInfoRepository.SearchPage(pageInfo, whereConditions.ToArray());
            //    //result.total = pageInfo.Total;
            //    //foreach(var item in list)
            //    //{
            //    //    result.rows.Add(new DTOUserInfo
            //    //    {
            //    //        ID = item.ID,
            //    //        IsPublish=item.IsPublish,
            //    //        VersionNo=item.VersionNo,
            //    //        PublishDate=item.PublishDate,
            //    //        LastPublishTime=item.LastPublishTime,
            //    //        LastOperateUser=item.LastOperateUser,
            //    //        LastUpdateTime=item.LastUpdateTime
            //    //    });
            //    //}
            //}
            var bll = new Repository<Prc_UserInfo>(DbContext);
            var list = bll.SearchPage(pageInfo, whereConditions.ToArray());
            result.total = pageInfo.Total;
            foreach (var item in list)
            {
                var userdto =(new DTOUserInfo
                {
                    ID = item.ID,
                    LoginName = item.LoginName,
                    UserName = item.UserName,
                    RightsDesc = "" 
                });
               var listRight = item.Rights.Split(",");
                string rDesc = "";
                foreach(string m in listRight)
                {
                    if(!string.IsNullOrEmpty(m))
                    {
                        rDesc += item.GetRightName(m)+",";
                    }

                }
                if (rDesc.Length > 1) rDesc = rDesc.Substring(0, rDesc.Length - 1);
                userdto.RightsDesc = rDesc;
                result.rows.Add(userdto);
            }
            return new JsonResult(result);
        }

        [HttpPost]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new CustomExecption("9999", "ID不能为空");
            }
            Repository<Prc_UserInfo> repos = new Repository<Prc_UserInfo>(DbContext);
         
            Prc_UserInfo model = repos.GetBaseID(Guid.Parse(id));
            if (model == null) throw new CustomExecption("9999", $"ID:{id}找不到用户");
            if (model.IsDelete) throw new CustomExecption("9999", $"用户已删除");
            DTOGetUserInfo res = new DTOGetUserInfo();
            res.ID = model.ID;
            res.LoginName = model.LoginName;
            res.UserName = model.UserName;
            res.PassWord = model.PassWord;
            res.Rights = model.Rights;
            return Json(res);
        }

        [HttpPost]
        [CustomActionFilter(ModelCode = "1003")]
        public IActionResult Add(DTOModUserInfo dto)
        {
            var result = new ReturnResponse();
            dto.LoginName = dto.LoginName.Trim();
            dto.UserName = dto.UserName.Trim();
            dto.PassWord = dto.PassWord.Trim();

            CheckUserInfo(dto);
            Repository<Prc_UserInfo> repos = new Repository<Prc_UserInfo>(DbContext);

            if (repos.IsExist(o => o.LoginName == dto.LoginName && !o.IsDelete))
            {
                throw new CustomExecption("9999", $"已经存在{dto.LoginName}的用户名");
            }
            var model = new Prc_UserInfo();
            model.LoginName = dto.LoginName;
            model.UserName = dto.UserName;
            model.PassWord = dto.PassWord;
            model.Rights = dto.Rights;
            if (model.Rights == null) model.Rights = "";
            model.IsDelete = false;
            repos.Add(model);
            DbContext.SaveChanges();
            result.ResponseStatus.Message = model.ID.ToString();
            return Json(result);

        }

        [HttpPost]
        public IActionResult Edit(DTOModUserInfo dto)
        {
            var result = new ReturnResponse();
            dto.LoginName = dto.LoginName.Trim();
            dto.UserName = dto.UserName.Trim();
            dto.PassWord = dto.PassWord.Trim();

            CheckUserInfo(dto);
            Repository<Prc_UserInfo> repos = new Repository<Prc_UserInfo>(DbContext);

            if (repos.IsExist(o => o.LoginName == dto.LoginName && o.ID != dto.ID && !o.IsDelete))
            {
                throw new CustomExecption("9999", $"已经存在{dto.LoginName}的用户名");
            }
            if (dto.ID == null || dto.ID == Guid.Empty) throw new CustomExecption("9999", $"程序错误，用户ID:为空");
            Prc_UserInfo model = repos.GetBaseID(dto.ID);
            if (model == null) throw new CustomExecption("9999", $"ID:{dto.ID.ToString()}找不到用户");
            if (model.IsDelete) throw new CustomExecption("9999", $"用户已经删除");
            model.LoginName = dto.LoginName;
            model.UserName = dto.UserName;
            model.PassWord = dto.PassWord;
            model.Rights = dto.Rights;
            if (model.Rights == null) model.Rights = "";
            model.IsDelete = false;
            repos.Update(model);
            DbContext.SaveChanges();
            return Json(result);

        }
        [HttpPost]
        public IActionResult Del(Guid id)
        {

            Repository<Prc_UserInfo> repos = new Repository<Prc_UserInfo>(DbContext);
            if (id == Guid.Empty) throw new CustomExecption("9999", $"程序错误，用户ID:为空");
            Prc_UserInfo model = repos.GetBaseID(id);
            if (model == null) throw new CustomExecption("9999", $"ID:{id.ToString()}找不到用户");
            if (model.IsDelete) throw new CustomExecption("9999", $"用户已经删除");
            model.IsDelete = true;
            repos.Update(model);
            DbContext.SaveChanges();

            return Json(new ReturnResponse());
        }
        [HttpPost]
        public IActionResult UpdatePass(DTOUpdateUserPass dto)
        {
            var result = new ReturnResponse();
            if (string.IsNullOrEmpty(dto.OldPass.Trim())) throw new CustomExecption("9999", $"当前密码不能为空");
            if (string.IsNullOrEmpty(dto.NewPass.Trim())) throw new CustomExecption("9999", $"新密码不能为空");
            if (dto.OldPass.Trim() == dto.NewPass.Trim()) throw new CustomExecption("9999", $"当前密码与新密码不可一致");
            var str = HttpContext.Session.GetString(ConstPara.SESSION_KEY);
            var uInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<UserSession>(str);

            Repository<Prc_UserInfo> repos = new Repository<Prc_UserInfo>(DbContext);
            Prc_UserInfo model = repos.GetBaseID(uInfo.ID);
            if (model == null) throw new CustomExecption("9999", $"ID:{uInfo.ID.ToString()}找不到用户");
            if (model.IsDelete) throw new CustomExecption("9999", $"用户已经删除");
            if (model.PassWord != dto.OldPass.Trim()) throw new CustomExecption("9999", $"当前密码不正确");
            model.PassWord = dto.NewPass.Trim();
            repos.Update(model);
            DbContext.SaveChanges();
            return Json(result);

        }


        private void CheckUserInfo(DTOModUserInfo dto)
        {
            string errorCode = "9999";
            if (string.IsNullOrWhiteSpace(dto.LoginName))
            {
                throw new CustomExecption(errorCode, "用户名不能为空");
            }
             
            if (string.IsNullOrWhiteSpace(dto.UserName))
            {
                throw new CustomExecption(errorCode, "姓名不能为空");
            }
            if (string.IsNullOrWhiteSpace(dto.PassWord))
            {
                throw new CustomExecption(errorCode, "密码不能为空");
            }
            //if (!string.IsNullOrEmpty(dto.ID))
            //{
            //    Guid id = Guid.Empty;
            //    if(!Guid.TryParse(dto.ID,out id))
            //    {
            //        throw new CustomExecption(errorCode, "ID的格式不正确");
            //    }
            //}
        }
    }
}