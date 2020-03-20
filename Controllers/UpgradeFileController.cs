using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using ProductMange.BLL.OneKeyUpgrade;
using ProductMange.DTO.UpgradeFile;
using ProductMange.Model;
using ProductMange.Public;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Controllers
{
    [CustomActionFilter(ModelCode = "1002")]
    public class UpgradeFileController : BaseControllerLogin
    {
        IMemoryCache _cache;
        public ProductManageDbContext DbContext { get; }
        public IBLLUpgrade BllUpgrade { get; }
        public UpgradeFileController(IMemoryCache cache,ProductManageDbContext _DbContext, IBLLUpgrade _BllUpgrade)
        {
            _cache = cache;
            DbContext = _DbContext;
            BllUpgrade = _BllUpgrade;
        }
        
        [HttpPost]
        [Route("/UpgradeFile/UploadUpgradeBag")]
        public ReturnResponse UploadUpgradeBag(DTOUpgradeFileUpload dto)
        {
            try
            {
                string fileDirPath = Environment.CurrentDirectory + "/LocalFile/UpgradeBag/";
                string fileFullName = fileDirPath + dto.FileName;
                if (!Directory.Exists(fileDirPath))
                {
                    Directory.CreateDirectory(fileDirPath);
                }
                if (dto.CurrPosition == 0 && System.IO.File.Exists(fileFullName))
                {
                    System.IO.File.Delete(fileFullName);
                }

                using (FileStream fs = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, dto.FileSize))
                {
                    fs.Position = dto.CurrPosition;
                    fs.Write(dto.FileBuffer.ToArray(), 0, dto.FileBuffer.Count);
                    if (fs.Length == dto.FileSize)
                    {
                        //上传完成
                        Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
                        Prc_VersionInfo versionInfo = repository.Get(a => a.ID == dto.VersionId);
                        versionInfo.UpgradeBagName = dto.FileName;
                        repository.Update(versionInfo);
                        OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "上传了版本【" + versionInfo.VersionNo + "】的更新包");
                        DbContext.SaveChanges();

                    }
                }
                return new ReturnResponse();
            }
            catch (Exception ex)
            {
                return new ReturnResponse() { ResponseStatus = new ResponseStatus() { ErrorCode = "999", Message = "上传失败:" + ex.Message } };
            }
        }

        [HttpPost]
        [Route("/UpgradeFile/DeleteUpgradeBag")]
        public ReturnResponse DeleteUpgradeBag(DTOUpgradeFileDelete dto)
        {
            string fileDirPath = Environment.CurrentDirectory + "/LocalFile/UpgradeBag/";


            Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
            Prc_VersionInfo versionInfo = repository.Get(a => a.ID == dto.VersionId);
            string fileFullName = fileDirPath + versionInfo.UpgradeBagName;
            string bytesKey = "UpgradeBag_" + dto.VersionId.ToString().ToUpper();
            string nameKey = "UpgradeBagName_" + dto.VersionId.ToString().ToUpper();
            _cache.Remove(bytesKey);
            _cache.Remove(nameKey);
            if (System.IO.File.Exists(fileFullName))
            {
                System.IO.File.Delete(fileFullName);
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

        [HttpGet]
        [Route("/UpgradeFile/DownloadUpgradeBag")]
        public IActionResult DownloadUpgradeBag(DTOUpgradeFileDownload dto)
        {
            string fileDirPath = Environment.CurrentDirectory + "/LocalFile/UpgradeBag/";

            string fileFullName;
            Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
            Prc_VersionInfo versionInfo = repository.Get(a => a.ID == dto.VersionId);
            fileFullName = fileDirPath + versionInfo.UpgradeBagName;
            if (!System.IO.File.Exists(fileFullName))
            {
                throw new Exception("未找到文件");
            }

            var stream = System.IO.File.OpenRead(fileFullName);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[".zip"];
            return File(stream, memi, Path.GetFileName(fileFullName));
        }
    }
}
