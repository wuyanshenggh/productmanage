using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductMange.Model;
using System.Text;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using ProductMange.Public;
using ProductMange.BLL.OneKeyUpgrade;
using ProductMange.DTO.Version;
using System.Security.Cryptography;
using Newtonsoft.Json;
using ProductMange.DTO.UpgradeFile;
using Microsoft.AspNetCore.Authorization;

namespace ProductMange.Controllers
{


    
    public class VersionController : BaseControllerLogin
    {
        static Loger logger = new Loger("OperateLog");

        public ProductManageDbContext DbContext { get; }


        public IBLLUpgrade BllUpgrade { get; }


        public VersionController(ProductManageDbContext _DbContext, IBLLUpgrade _BllUpgrade)
        {
            DbContext = _DbContext;
            BllUpgrade = _BllUpgrade;
        }


        [HttpPost]        
        [AllowAnonymous]
        [Route("/Version/Search")]
        //[CustomActionFilter(ModelCode = "1001")]
        public IActionResult Search([FromBody]DTOGetVersionInfoListByPage dto)
        {
            DTOBackVersionInfo result = new DTOBackVersionInfo();
            result.rows = new List<DTOVersionInfo>();
            PageInfo pageInfo = new PageInfo { OffSet=dto.OffSet,PageSize=dto.PageSize,Sort=dto.Sort,SortOrder=dto.SortOrder};
            List<Expression<Func<Prc_VersionInfo, bool>>> whereConditions = new List<Expression<Func<Prc_VersionInfo, bool>>>();
            if (!string.IsNullOrWhiteSpace(dto.VersionNo))
            {
                whereConditions.Add(o => o.VersionNo.Contains(dto.VersionNo));
            }
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            if (!string.IsNullOrWhiteSpace(dto.StartTime))
            {
                if(!DateTime.TryParse(dto.StartTime,out startTime))
                {
                    throw new CustomExecption("9999", $"更新开始日期格式错误");
                }
                whereConditions.Add(o => o.LastUpdateTime >= startTime);
            }
            if (!string.IsNullOrWhiteSpace(dto.EndTime))
            {
                if (!DateTime.TryParse(dto.EndTime, out endTime))
                {
                    throw new CustomExecption("9999", $"更新结束日期格式错误");
                }
                endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day).AddDays(1);
                whereConditions.Add(o => o.LastUpdateTime<endTime );
            }
            whereConditions.Add(o => !o.IsDelete);
            using (var unitOfWork = new UnitOfWork(DbContext))
            {
                var list = unitOfWork.VersionInfoRepository.SearchPage(pageInfo, whereConditions.ToArray());
                result.total = pageInfo.Total;
                foreach(var item in list)
                {
                    result.rows.Add(new DTOVersionInfo {
                        ID = item.ID,
                        IsPublish = item.IsPublish,
                        VersionNo = item.VersionNo,
                        PublishDate = item.PublishDate,
                        LastPublishTime = item.LastPublishTime,
                        LastOperateUser = item.LastOperateUser,
                        LastUpdateTime = item.LastUpdateTime,
                        UpgradeBagName = item.UpgradeBagName
                    });
                }
            }

            return new JsonResult(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/Get")]
        public IActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new CustomExecption("9999", "ID不能为空");
            }
            using (var unitOfWork=new UnitOfWork(DbContext))
            {
                var model = unitOfWork.VersionInfoRepository.Get(o => o.ID == Guid.Parse(id)&&!o.IsDelete);
                if (model == null)
                {
                    throw new CustomExecption("9999", "获取版本信息失败");
                }

                return Json(new DTOBackVersionDetail{ ID=model.ID,VersionNo=model.VersionNo,PublishDate=model.PublishDate,Content=Encoding.UTF8.GetString(model.Context)});
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/Add")]
        public IActionResult Add(DTOSaveVersionInfo dto)
        {
            var result = new ReturnResponse();
            CheckVersionInfo(dto);
            using (var unitOfWork=new UnitOfWork(DbContext))
            {
                if (unitOfWork.VersionInfoRepository.IsExist(o => o.VersionNo == dto.VersionNo&&!o.IsDelete))
                {
                    throw new CustomExecption("9999", $"已经存在版本号为{dto.VersionNo}的版本信息");
                }
                var model = new Prc_VersionInfo();
                model.ID = Guid.NewGuid();
                model.VersionNo = dto.VersionNo;
                model.PublishDate = Convert.ToDateTime(dto.PublishDate).RemoveMilliSecond();
                model.IsPublish = false;
                model.Context = Encoding.UTF8.GetBytes(dto.Content);
                model.LastOperateUser = LoginUserInfo.CurrUser.LoginName;
                model.LastUpdateTime = DateTime.Now.RemoveMilliSecond();
                model.IsDelete = false;
                unitOfWork.VersionInfoRepository.Add(model);
                OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "新增版本【" + dto.VersionNo + "】");
                unitOfWork.SaveChanges();
            }

            return Json(result);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/Edit")]
        public IActionResult Edit(DTOSaveVersionInfo dto)
        {
            CheckVersionInfo(dto);
            if (string.IsNullOrWhiteSpace(dto.ID))
            {
                throw new CustomExecption("9999", "ID不能为空");
            }
            ReturnResponse result = new ReturnResponse();
            using (var unitOfWork = new UnitOfWork(DbContext))
            {
                var model = unitOfWork.VersionInfoRepository.Get(o => o.ID == Guid.Parse(dto.ID)&&!o.IsDelete);
                if (model == null)
                {
                    throw new CustomExecption("9999", "获取版本信息失败");
                }
                if (unitOfWork.VersionInfoRepository.IsExist(o => o.VersionNo == dto.VersionNo&&o.ID!=model.ID&&!o.IsDelete))
                {
                    throw new CustomExecption("9999", $"已经存在版本号为{dto.VersionNo}的版本信息");
                }
                model.VersionNo = dto.VersionNo;
                model.PublishDate = Convert.ToDateTime(dto.PublishDate).RemoveMilliSecond();
                model.Context = Encoding.UTF8.GetBytes(dto.Content);
                model.LastOperateUser = LoginUserInfo.CurrUser.LoginName;
                model.LastUpdateTime = DateTime.Now.RemoveMilliSecond();
                if (model.IsPublish)
                {
                    // 如果是已发布的，则取消发布
                    model.IsPublish = false;
                }
                unitOfWork.VersionInfoRepository.Update(model);

                unitOfWork.SaveChanges();
            }

            return Json(result);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/Del")]
        public IActionResult Del(Guid id)
        {

            using (var unitOfWork = new UnitOfWork(DbContext))
            {
                var model = unitOfWork.VersionInfoRepository.Get(o => o.ID == id && !o.IsDelete);
                if (model == null)
                {
                    throw new CustomExecption("9999", "获取版本信息失败");
                }
                model.IsDelete = true;
                model.LastUpdateTime = DateTime.Now.RemoveMilliSecond();
                model.LastPublishTime = DateTime.Now.RemoveMilliSecond();
                unitOfWork.VersionInfoRepository.Update(model);

                //取消该版本的所有预约
                Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);
                List<Prc_UpgradeInfo> infos = repositoryInfo.Search(a => !a.IsDelete && a.UpgradeStatus == Model.Enum.EnumUpgradeStatus.Reserved && a.TargetVersionID == model.ID);           
                infos.ForEach(a =>
                {
                    BllUpgrade.CancelReserve(a.ID);
                });

                OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "删除版本【" + model.VersionNo + "】");
                DbContext.SaveChanges();
                unitOfWork.SaveChanges();
            }

            return Json(new ReturnResponse());
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/Publish")]
        public IActionResult Publish(Guid id)
        {
            using (var unitOfWork = new UnitOfWork(DbContext))
            {
                var model = unitOfWork.VersionInfoRepository.Get(o => o.ID == id&&!o.IsDelete);
                if (model == null)
                {
                    throw new CustomExecption("9999", "获取版本信息失败");
                }
                if (string.IsNullOrEmpty(model.UpgradeBagName))
                {
                    throw new CustomExecption("9999", "请先上传更新包");
                }

                model.IsPublish = true;
                model.LastPublishTime = DateTime.Now.RemoveMilliSecond();
                model.LastUpdateTime = DateTime.Now.RemoveMilliSecond();
                unitOfWork.VersionInfoRepository.Update(model);

                unitOfWork.SaveChanges();
            }

            return Json(new ReturnResponse());
        }

        private void CheckVersionInfo(DTOSaveVersionInfo dto)
        {
            string errorCode = "9999";
            if (string.IsNullOrWhiteSpace(dto.VersionNo))
            {
                throw new CustomExecption(errorCode, "版本号不能为空");
            }
            var regExp = new Regex(@"^[0-9]{2}.[0-9]{1}.[0-9]{4}$");
            if (!regExp.IsMatch(dto.VersionNo))
            {
                throw new CustomExecption(errorCode, "版本号格式不正确，格式必须类似于10.1.1001");
            }
            if (string.IsNullOrWhiteSpace(dto.PublishDate))
            {
                throw new CustomExecption(errorCode, "对外发布日期不能为空");
            }
            DateTime date = DateTime.Now;
            if (!DateTime.TryParse(dto.PublishDate, out date))
            {
                throw new CustomExecption(errorCode, "对外发布日期格式不正确");
            }
            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                throw new CustomExecption(errorCode, "版本说明不能为空");
            }
            byte[] bytes = Encoding.UTF8.GetBytes(dto.Content);
            if (bytes.Length > 2 * 1024 * 1024)
            {
                throw new CustomExecption(errorCode, "版本说明大小不能超过2M");
            }
            if (!string.IsNullOrEmpty(dto.ID))
            {
                Guid id = Guid.Empty;
                if(!Guid.TryParse(dto.ID,out id))
                {
                    throw new CustomExecption(errorCode, "ID的格式不正确");
                }
            }
        }

        #region 生成网易云token

        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/GetNosUploadInfo")]
        public DTOBackNosUploadInfo GetNosUploadInfo(DTOGetNosUploadInfo dto)
        {
            DateTime DateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan ts = DateTime.UtcNow.AddMinutes(30) - DateTime1970;
            int epoch = (int)ts.TotalSeconds;

            var putPolicy = new NosTokenPolicy() { Bucket = NosConfig.BucketName, Object = dto.ObjectName, Expires = epoch };
            string policyJson = JsonConvert.SerializeObject(putPolicy);
            string policyBase64 = ToBase64(policyJson);
            try
            {
                string signBase64 = ToHMACSHA256(policyBase64, NosConfig.SecretKey);
                //string signBase64 = ToBase64(key);
                string token = "UPLOAD " + NosConfig.AccessKey + ":" + signBase64 + ":" + policyBase64;
                return new DTOBackNosUploadInfo() { Token = token, BucketName = NosConfig.BucketName, EndPoint = NosConfig.EndPoint };
            }
            catch (Exception e)
            {
                return new DTOBackNosUploadInfo() { ResponseStatus = new ResponseStatus() { ErrorCode = "999", Message = "获取上传信息失败：" + e.Message } };
            }
        }

        private string ToBase64(string text)
        {
            byte[] b = System.Text.Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(b, 0, b.Length);
        }

        private string ToHMACSHA256(string encryptText, string encryptKey)
        {
            //HMACSHA256加密 
            byte[] keyByte = System.Text.Encoding.UTF8.GetBytes(encryptKey);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(encryptText));
                return Convert.ToBase64String(hashmessage);
            }
        }

        #endregion


        [HttpPost]
        [AllowAnonymous]
        [Route("/Version/SetUpgradeBag")]
        public IActionResult SetUpgradeBag(DTOUpgradeName dto)
        {

            //上传完成
            Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
            Prc_VersionInfo versionInfo = repository.Get(a => a.ID == dto.VersionId);
            versionInfo.UpgradeBagName = dto.BagName;
            repository.Update(versionInfo);
            OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "上传了版本【" + versionInfo.VersionNo + "】的更新包");
            DbContext.SaveChanges();
            return Json(new ReturnResponse());
        }

        [HttpPost]
        [Route("/Version/DeleteUpgradeBag")]
        public ReturnResponse DeleteUpgradeBag(DTOUpgradeFileDelete dto)
        {
            string fileDirPath = Environment.CurrentDirectory + "/LocalFile/UpgradeBag/";

            Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
            Prc_VersionInfo versionInfo = repository.Get(a => a.ID == dto.VersionId);
            try
            {
                NosService.DeleteFile("UpgradeBag/" + versionInfo.UpgradeBagName);
            }
            catch (Exception ex)
            {
                logger.Error("删除升级文件失败", ex);
            }

            versionInfo.IsPublish = false;
            versionInfo.LastPublishTime = null;
            versionInfo.UpgradeBagName = string.Empty;
            repository.Update(versionInfo);

            //取消该版本的所有预约
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);
            List<Prc_UpgradeInfo> infos = repositoryInfo.Search(a => !a.IsDelete && a.UpgradeStatus == Model.Enum.EnumUpgradeStatus.Reserved && a.TargetVersionID == versionInfo.ID);
        
            infos.ForEach(a =>
            {
                BllUpgrade.CancelReserve(a.ID);
            });

            OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "删除了版本【" + versionInfo.VersionNo + "】的更新包");
            DbContext.SaveChanges();

            return new ReturnResponse();
        }
    }
}