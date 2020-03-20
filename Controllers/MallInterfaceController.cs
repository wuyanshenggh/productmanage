using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ProductMange.DTO.Mall;
using ProductMange.Model;
using ProductMange.Model.Enum;
using ProductMange.Public;
using ProductMange.BLL.OneKeyUpgrade;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;

namespace ProductMange.Controllers
{
    /// <summary>
    /// 娱乐管家接口
    /// </summary>
    public class MallInterfaceController : OutInterfaceBaseController
    {
        IMemoryCache _cache;
        public ProductManageDbContext DbContext { get; }

        public IBLLUpgrade BllUpgrade { get; }

        public MallInterfaceController(IMemoryCache cache, ProductManageDbContext _DbContext, IBLLUpgrade _BllUpgrade)
        {
            _cache = cache;
            DbContext = _DbContext;
            BllUpgrade = _BllUpgrade;

        }
        /// <summary>
        /// 获取假期日历
        /// </summary>        
        [HttpGet]
        [Route("/MallInterface/GetHolidayCalendar")]
        public DTOBackHolidayCalendar GetHolidayCalendar([FromBody]DTOGetHolidayCalendar dto)
        {
            DTOBackHolidayCalendar result = new DTOBackHolidayCalendar();
            result.HolidayJsons = new Dictionary<int, string>();

            Repository<Prc_Holiday> repository = new Repository<Prc_Holiday>(DbContext);
            List<Prc_Holiday> holidays = repository.Search(a => !a.IsDelete && dto.Years.Contains(a.Year));
            holidays.ForEach(a =>
            {
                if (!result.HolidayJsons.ContainsKey(a.Year))
                {
                    result.HolidayJsons.Add(a.Year, a.Data);
                }
            });

            Repository<Prc_ConfigSet> repositoryCfgSet = new Repository<Prc_ConfigSet>(DbContext);
            var revDay = repositoryCfgSet.Get(a => a.Code == PrcConfigSet.ReserveDay);
            var revHour = repositoryCfgSet.Get(a => a.Code == PrcConfigSet.ReserveHour);
            result.ReserveDay = revDay == null || string.IsNullOrWhiteSpace(revDay.Value) ? 7 : int.Parse(revDay.Value);
            result.ReserveHour = revHour == null || string.IsNullOrWhiteSpace(revHour.Value) ? 24 : int.Parse(revHour.Value);
            return result;

        }
        /// <summary>
        /// 获取当前最新版本信息
        /// </summary>
        [HttpGet]
        [Route("/MallInterface/GetPfVersionInfo")]
        public DTOBackNewestVersionInfo GetPfVersionInfo()
        {
          
            DTOBackNewestVersionInfo result = new DTOBackNewestVersionInfo();
            Prc_VersionInfo verInfo = BllUpgrade.GetNewestVersionInfo();
            if (verInfo == null)
            {
                result.ResponseStatus.ErrorCode = "999";
                result.ResponseStatus.Message = "未找到版本信息";
                return result;
            }
            result.VersionID = verInfo.ID;
            result.VersionNo = verInfo.VersionNo;
            return result;
        }
     
        /// <summary>
        /// 获取当前最新版本信息
        /// </summary>
        [HttpGet]
        [Route("/MallInterface/GetVersionInfoById")]
        public DTOBackVersionInfoById GetVersionInfoById([FromBody]DTOGetVersionInfoById dto)
        {
            
            DTOBackVersionInfoById result = new DTOBackVersionInfoById();
            Prc_VersionInfo verInfo = BllUpgrade.GetVersionInfoById(dto.VersionId);
            if (verInfo == null)
            {
                result.ResponseStatus.ErrorCode = "999";
                result.ResponseStatus.Message = "未找到版本信息";
                return result;
            }
            result.VersionID = verInfo.ID;
            result.VersionNo = verInfo.VersionNo;
            result.UpgradeBagName = verInfo.UpgradeBagName;
            return result;
        }

        /// <summary>
        /// 预约升级接口
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/MallInterface/ReserveUpgrade")]
        public DTOBackReserveUpgrade ReserveUpgrade([FromBody]DTOReserveUpgrade dto)
        {
            if (dto.BusinessInfos == null || dto.BusinessInfos.Count == 0)
            {
                throw new Exception("门店信息不能为空");
            }
            DTOBackReserveUpgrade result = new DTOBackReserveUpgrade();
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(DbContext);
            Repository<Prc_VersionInfo> repositoryVer = new Repository<Prc_VersionInfo>(DbContext);

            var oldInfo = repositoryInfo.Get(a => a.MallCode == dto.MallCode && !a.IsDelete && (a.UpgradeStatus == EnumUpgradeStatus.Reserved || a.UpgradeStatus == EnumUpgradeStatus.Upgrading));

            if (oldInfo != null)
            {
                if (oldInfo.TargetVersionID != dto.TargetVersionID)
                {
                    throw new Exception("平台存在未完成的预约");
                }
                else
                {
                    return new DTOBackReserveUpgrade() { InfoID = oldInfo.ID };
                }
            }

            Prc_VersionInfo upgradeVer = repositoryVer.Get(ver => ver.ID == dto.TargetVersionID && !ver.IsDelete);
            if (upgradeVer == null)
            {
                throw new Exception("未找到版本信息");
            }

            Prc_UpgradeInfo info = new Prc_UpgradeInfo()
            {
                BusinessCount = dto.BusinessInfos.Count,
                ContactPerson = dto.ContactPerson,
                ContactPhone = dto.ContactPhone,
                MallCode = dto.MallCode,
                MallName = dto.MallName,
                OriginalVersionNo = dto.OriginalVersionNo,
                ReserveTime = dto.ReserveTime,
                TargetVersionNo = dto.TargetVersionNo,
                TargetVersionID = upgradeVer.ID,
                CreateOperateUser = BLLUpgrade.SystemUser,
                UpgradeStatus = EnumUpgradeStatus.Reserved,
                HeartbeatTime = DateTime.Now,
                IsSingle = dto.IsSingle,
                ApplyTime = dto.ApplyTime
            };
            repositoryInfo.Add(info);
            dto.BusinessInfos.ForEach(a =>
            {
                Prc_UpgradeInfoItem item = new Prc_UpgradeInfoItem()
                {
                    BusinessNum = a.BusinessNum,
                    BusinessType = (EnumBusinessType)a.BusinessType,
                    CreateOperateUser = BLLUpgrade.SystemUser,
                    BusinessName = a.BusinessName,
                    HeartbeatStatus = -1,
                    UpgradeInfo = info,
                    UpgradeStatus = EnumUpgradeStatus.Reserved,
                    UpgradeBagStatus = EnumUpgradeBagStatus.None
                };
                repositoryItem.Add(item);
            });
            result.InfoID = info.ID;
            DbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// 升级前信息交互及心跳
        /// </summary>
        [HttpPost]
        [HttpGet]
        [Route("/MallInterface/Heartbeat")]
        public DTOBackHeartbeat Heartbeat([FromBody]DTOHeartbeat dto)
        {
            DTOBackHeartbeat restul = new DTOBackHeartbeat();
            restul.CancelBusiness = new List<string>();
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);
            Prc_UpgradeInfo info = repositoryInfo.Get(a => a.ID == dto.InfoID && !a.IsDelete);
            if (info == null)
            {
                throw new Exception("未找到升级信息");
            }
            //写入信息的心跳时间
            info.HeartbeatTime = DateTime.Now;
            info.LastOperateUser = BLLUpgrade.SystemUser;
            repositoryInfo.Update(info);


            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(DbContext);
            List<Prc_UpgradeInfoItem> items = repositoryItem.Search(a => a.UpgradeInfo.ID == info.ID && !a.IsDelete);
            items.ForEach(a =>
            {
                    //写入子项的心跳状态
                    if (dto.LostNetworkBusNum != null && dto.LostNetworkBusNum.Contains(a.BusinessNum))
                {
                    a.HeartbeatStatus = 1;
                }
                else
                {
                    a.HeartbeatStatus = 0;
                }

                a.LastOperateUser = BLLUpgrade.SystemUser;
                repositoryItem.Update(a);

                if (a.UpgradeStatus == EnumUpgradeStatus.Canceled)
                {
                        //被忽略/取消的门店
                        restul.CancelBusiness.Add(a.BusinessNum);
                }
            });

            DbContext.SaveChanges();

            //赋返回信息
            restul.ConfirmUpgradeTime = info.ConfirmUpgradeTime;
            restul.IsCancel = info.UpgradeStatus == EnumUpgradeStatus.Canceled;
            restul.IsStartUpgrade = info.UpgradeStatus == EnumUpgradeStatus.Upgrading;


            return restul;
        }

        /// <summary>
        /// 写入升级进度消息 
        /// </summary>
        [HttpPost]
        [Route("/MallInterface/ReceiveMessage")]
        public DTOBackMallMessage ReceiveMessage([FromBody]DTOMallMessage dto)
        {
            DTOBackMallMessage result = new DTOBackMallMessage();
            result.FailMessages = new Dictionary<Guid, string>();
            if (dto.Messages == null || dto.Messages.Count == 0)
            {
                return result;
            }         
            List<Prc_UpgradeMessage> mallMegs = new List<Prc_UpgradeMessage>();
            dto.Messages.ForEach(a =>
            {
                Prc_UpgradeMessage msg = new Prc_UpgradeMessage()
                {
                    ID = a.ID,
                    BusinessNum = a.BusinessNum,
                    Content = a.Content,
                    MessageType = (EnumMessageType)a.MessageType,
                    MessageFlag = a.MessageFlag,
                    OccurTime = a.OccurTime,
                    CreateOperateUser = BLLUpgrade.SystemUser,
                    LastUpdateTime = DateTime.Now,
                    CreateTime = DateTime.Now,
                    HandleStatus = EnumHandleStatus.UnHandle
                };
                mallMegs.Add(msg);
            });
            result.FailMessages = BllUpgrade.SaveMallMessage(mallMegs, dto.PfInfoId);
            return result;
        }
        [NonAction]
        /// <summary>
        /// 获取询问消息的结果
        /// </summary>
        [HttpPost]
        [HttpGet]
        [Route("/MallInterface/GetMessageResult")]
        public DTOBackMallMessageResult GetMessageResult([FromBody]DTOGetMallMessageResult dto)
        {
            DTOBackMallMessageResult result = new DTOBackMallMessageResult();
            result.Results = new Dictionary<Guid, string>();
            if (dto.MessageIds == null && dto.MessageIds.Count == 0)
            {
                return result;
            }

            Repository<Prc_UpgradeMessage> repository = new Repository<Prc_UpgradeMessage>(DbContext);
            dto.MessageIds.ForEach(msgId =>
            {
                var msg = repository.Get(b => b.ID == msgId && b.HandleStatus == EnumHandleStatus.Handled && !b.IsDelete);
                if (msg != null)
                {
                    result.Results.Add(msgId, msg.Result);
                }
            });

            return result;
        }
        [HttpPost]
        [HttpGet]
        [Route("/MallInterface/CheckDataQueue")]
        public DTOBackCheckDataQueue CheckDataQueue([FromBody]DTOCheckDataQueue dto)
        {
            bool finished = true;
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(DbContext);
            Repository<Prc_UpgradeMessage> repositoryMsg = new Repository<Prc_UpgradeMessage>(DbContext);
            List<Prc_UpgradeInfoItem> items = repositoryItem.Search(a => a.UpgradeInfo.ID == dto.InfoId &&
                                                (a.UpgradeStatus == EnumUpgradeStatus.Reserved || a.UpgradeStatus == EnumUpgradeStatus.Upgrading) && !a.IsDelete);

            foreach (Prc_UpgradeInfoItem fhItem in items)
            {
                if (repositoryMsg.Get(a => !a.IsDelete && a.UpgradeInfoItem.ID == fhItem.ID && a.MessageFlag == InfoMsgFlag.DataSynTaskFinish) == null)
                {
                    finished = false;
                    break;
                }
            }

            return new DTOBackCheckDataQueue() { Finished = finished };
        }

        private static object secDlUpgradeBagLock = new object();
        /// <summary>
        /// 分段下载更新包
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("/MallInterface/SectionDownloadUpgradeBag")]
        public DTOBackFileSection SectionDownloadUpgradeBag([FromBody]DTOUpgradeFileSectDownload dto)
        {
            //缓存
            string bytesKey = "UpgradeBag_" + dto.VersionId.ToString().ToUpper();
            string nameKey = "UpgradeBagName_" + dto.VersionId.ToString().ToUpper();
            List<byte> bytes = _cache.Get<List<byte>>(bytesKey);
            if (bytes == null)
            {
                Prc_VersionInfo versionInfo;

                Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
                versionInfo = repository.Get(a => a.ID == dto.VersionId);

                if (versionInfo == null)
                {
                    throw new Exception("未找到版本信息");
                }
                string fileFullName = Environment.CurrentDirectory + "/LocalFile/UpgradeBag/" + versionInfo.UpgradeBagName;
                if (!System.IO.File.Exists(fileFullName))
                {
                    throw new Exception("未找到文件");
                }

                //using ()
                using (FileStream fs = System.IO.File.OpenRead(fileFullName))
                {
                    byte[] bs = new byte[fs.Length];
                    fs.Read(bs, 0, bs.Length);
                    bytes = bs.ToList();
                    //写入字节数组
                    _cache.GetOrCreate(bytesKey, entry =>
                    {
                        entry.SetSlidingExpiration(TimeSpan.FromHours(1));
                        return bytes;
                    });
                }
            }
            string fileName = _cache.Get<string>(nameKey);
            if (fileName == null)
            {
                Prc_VersionInfo versionInfo;

                Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
                versionInfo = repository.Get(a => a.ID == dto.VersionId);

                if (versionInfo == null)
                {
                    throw new Exception("未找到版本信息");
                }
                fileName = versionInfo.UpgradeBagName;
                _cache.GetOrCreate(nameKey, entry =>
                {
                    entry.SetSlidingExpiration(TimeSpan.FromHours(1));
                    return fileName;
                });
            }
            DTOBackFileSection result = new DTOBackFileSection();
            //fs.Position = dto.CurrPosition;
            result.CurrPosition = dto.CurrPosition;
            result.FileName = fileName;
            result.FileSize = bytes.Count;

            if (bytes.Count <= dto.CurrPosition)
            {
                result.CurrSize = 0;
                result.FileBuffer = new byte[] { };
            }
            else
            {
                if (bytes.Count <= dto.CurrPosition + dto.CurrSize)
                {
                    result.FileBuffer = bytes.GetRange((int)dto.CurrPosition, (int)(bytes.Count - dto.CurrPosition)).ToArray();
                    result.CurrSize = (int)(bytes.Count - dto.CurrPosition);
                }
                else
                {
                    result.FileBuffer = bytes.GetRange((int)dto.CurrPosition, dto.CurrSize).ToArray();
                    result.CurrSize = dto.CurrSize;
                }
            }

            return result;
        }

        /// <summary>
        /// 下载更新程序
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("/MallInterface/SectionDownloadUpgradeExe")]
        public DTOBackFileSection SectionDownloadUpgradeExe([FromBody]DTOUpgradeFileSectDownload dto)
        {
            string fileFullName = Environment.CurrentDirectory + "/LocalFile/UpgradeExe/UpgradeApp.zip";

            if (System.IO.File.Exists(fileFullName))
            {
                using (FileStream fs = System.IO.File.OpenRead(fileFullName))
                {
                    DTOBackFileSection result = new DTOBackFileSection();
                    fs.Position = dto.CurrPosition;
                    result.CurrPosition = dto.CurrPosition;
                    result.FileName = System.IO.Path.GetFileName(fs.Name);
                    result.FileSize = fs.Length;
                    result.FileBuffer = new byte[dto.CurrSize];
                    result.CurrSize = fs.Read(result.FileBuffer, 0, dto.CurrSize);
                    return result;
                }
            }
            else
            {
                throw new Exception("未找到升级程序");
            }
        }

        private static int _downloadInterval = -1;

        private static int DownloadInterval
        {
            get
            {
                if (_downloadInterval < 0)
                {
                    _downloadInterval = 5;
                    string di = ConfigurationUtil.GetSection("AppSettings:DownloadInterval");
                    if (!string.IsNullOrWhiteSpace(di))
                    {
                        try
                        {
                            _downloadInterval = int.Parse(di);
                        }
                        catch
                        { }
                    }
                }
                return _downloadInterval;
            }
        }     
        /// <summary>
        /// 通用下载接口
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("/MallInterface/SectionDownloadFile")]        
        public DTOBackFileSection SectionDownloadFile([FromBody]DTODownLoadFile dto)
        {
            string fileFullName = Environment.CurrentDirectory + "/LocalFile/" + dto.FileRelativePath;
            string requestIp = HttpContext.Connection.RemoteIpAddress.ToString();
            if (_cache.Get(requestIp) != null)
            {
                throw new Exception("请求频率过高!");
            }
            _cache.GetOrCreate<string>(requestIp, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(DownloadInterval));
                return string.Empty;
            });

            if (System.IO.File.Exists(fileFullName))
            {
                using (FileStream fs = System.IO.File.OpenRead(fileFullName))
                {
                    DTOBackFileSection result = new DTOBackFileSection();
                    fs.Position = dto.CurrPosition;
                    result.CurrPosition = dto.CurrPosition;
                    result.FileName = System.IO.Path.GetFileName(fs.Name);
                    result.FileSize = fs.Length;
                    result.FileBuffer = new byte[dto.CurrSize];
                    result.CurrSize = fs.Read(result.FileBuffer, 0, dto.CurrSize);
                    return result;
                }
            }
            else
            {
                throw new Exception("未找到文件");
            }
        }
    }
}
