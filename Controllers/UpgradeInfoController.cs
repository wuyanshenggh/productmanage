using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductMange.DTO.UpgradeInfo;
using ProductMange.Model;
using ProductMange.Model.Enum;
using ProductMange.Public;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProductMange.BLL.OneKeyUpgrade;

namespace ProductMange.Controllers
{




    [CustomActionFilter(ModelCode = "1002")]
    public class UpgradeInfoController: BaseControllerLogin
    {



        public ProductManageDbContext DbContext { get; }


        public IBLLUpgrade BllUpgrade { get; }


        public UpgradeInfoController(ProductManageDbContext _DbContext, IBLLUpgrade _BllUpgrade)
        {
            DbContext = _DbContext;
            BllUpgrade = _BllUpgrade;
        }



        /// <summary>
        /// 查询升级信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DTOBackUpgradeInfoByPage SearchUpgradeInfo([FromBody]DTOGetUpgradeInfoByPage dto)
        {
            DTOBackUpgradeInfoByPage result = new DTOBackUpgradeInfoByPage();
            result.rows = new List<UpgradeInfo>();
            PageInfo pageInfo = new PageInfo { OffSet = dto.OffSet, PageSize = dto.PageSize, Sort = dto.Sort, SortOrder = dto.SortOrder };
            List<Expression<Func<Prc_UpgradeInfo, bool>>> whereConditions = new List<Expression<Func<Prc_UpgradeInfo, bool>>>();
            if (!string.IsNullOrWhiteSpace(dto.MallName))
            {
                whereConditions.Add(o => o.MallName.Contains(dto.MallName));
            }
            if (!string.IsNullOrWhiteSpace(dto.StartConfirmTime))
            {
                DateTime startTime = DateTime.Now;
                if (!DateTime.TryParse(dto.StartConfirmTime, out startTime))
                {
                    throw new CustomExecption("9999", $"确认更新开始日期格式错误");
                }
                whereConditions.Add(o => o.ConfirmUpgradeTime >= startTime);
            }
            if (!string.IsNullOrWhiteSpace(dto.EndConfirmTime))
            {
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(dto.EndConfirmTime, out endTime))
                {
                    throw new CustomExecption("9999", $"确认更新结束日期格式错误");
                }
                endTime = new DateTime(endTime.Year, endTime.Month, endTime.Day).AddDays(1);
                whereConditions.Add(o => o.ConfirmUpgradeTime < endTime);
            }
            if (dto.UpgradeStatus > 0)
            {
                whereConditions.Add(o => o.UpgradeStatus == (EnumUpgradeStatus)dto.UpgradeStatus);
            }
            whereConditions.Add(o => !o.IsDelete);          
                Repository<Prc_UpgradeInfo> repository = new Repository<Prc_UpgradeInfo>(DbContext);
                var list = repository.SearchPage(pageInfo, whereConditions.ToArray());
                result.total = pageInfo.Total;
                List<string> upgradingIds = new List<string>();
                Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(DbContext);
                foreach (var fhItem in list)
                {
                    //基本信息
                    UpgradeInfo info = new UpgradeInfo()
                    {
                        ID = fhItem.ID,
                        BusinessCount = fhItem.BusinessCount,
                        ConfirmUpgradeTime = fhItem.ConfirmUpgradeTime.HasValue ? fhItem.ConfirmUpgradeTime.GetValueOrDefault().ToString("yyyy-MM-dd HH:mm") : string.Empty,
                        ContactPerson = fhItem.ContactPerson,
                        ContactPhone = fhItem.ContactPhone,
                        MallCode = fhItem.MallCode,
                        MallName = fhItem.MallName,
                        OriginalVersion = fhItem.OriginalVersionNo,
                        TargetVersion = fhItem.TargetVersionNo,
                        UpgradeStatus = (int)fhItem.UpgradeStatus,
                        ReserveTime = fhItem.ReserveTime,
                        Summary = fhItem.Summary
                    };

                    //耗时与心跳状态
                    if (fhItem.UpgradeStatus == Model.Enum.EnumUpgradeStatus.Upgrading || fhItem.UpgradeStatus == EnumUpgradeStatus.Reserved)
                    {
                        info.TimeConsum = fhItem.StartUpgradeTime.HasValue ? (int)(DateTime.Now - fhItem.StartUpgradeTime.GetValueOrDefault()).TotalMinutes : 0;


                        if ((DateTime.Now - fhItem.HeartbeatTime).TotalMinutes > 15)
                        {
                            info.HeartbeatStatus = 1;
                        }
                        else
                        {
                            if (repositoryItem.Get(a => !a.IsDelete && a.UpgradeInfo.ID == fhItem.ID && a.HeartbeatStatus == 1 &&
                                    (a.UpgradeStatus == EnumUpgradeStatus.Upgrading || a.UpgradeStatus == EnumUpgradeStatus.Reserved)) == null)
                            {
                                info.HeartbeatStatus = 0;
                            }
                            else
                            {
                                info.HeartbeatStatus = 1;
                            }
                        }
                    }
                    else if (fhItem.UpgradeStatus == Model.Enum.EnumUpgradeStatus.UpgradeFail || fhItem.UpgradeStatus == Model.Enum.EnumUpgradeStatus.UpgradeSucc)
                    {
                        ///更新成功或失败
                        info.TimeConsum = (int)(fhItem.EndUpgradeTime.GetValueOrDefault() - fhItem.StartUpgradeTime.GetValueOrDefault()).TotalMinutes;
                        info.HeartbeatStatus = -1;
                    }
                    else
                    {
                        info.TimeConsum = 0;
                        info.HeartbeatStatus = -1;
                    }

                    if (fhItem.UpgradeStatus == EnumUpgradeStatus.Upgrading || fhItem.UpgradeStatus == EnumUpgradeStatus.Reserved || fhItem.UpgradeStatus == EnumUpgradeStatus.UpgradeFail)
                    {
                        upgradingIds.Add(fhItem.ID.ToString());
                    }
                    result.rows.Add(info);
                }
            if (upgradingIds.Count > 0)
            {
                //异常个数
                string sqlText = string.Format(@"select info.ID as id,count(0) as ACount from Prc_UpgradeInfoItem item,Prc_UpgradeMessage msg,Prc_UpgradeInfo info
                                                        where item.ID = msg.UpgradeInfoItemID
                                                        and item.UpgradeInfoID = info.ID
                                                        and info.IsDelete = 0
                                                        and item.IsDelete = 0
                                                        and msg.IsDelete = 0
                                                        and ((msg.MessageType = 2 and msg.HandleStatus = 1) or MessageType = 3)
                                                        and msg.MessageFlag != 'UPGRADE_CANCEL'
                                                        and info.ID in ('" + string.Join("','", upgradingIds) + "') group by info.ID;");
                DataTable dt = repository.SearchBySql(sqlText);
                foreach (DataRow fhDr in dt.Rows)
                {
                    int aCount = (int)fhDr["ACount"];
                    if (aCount == 0)
                    {
                        continue;
                    }

                    Guid id = (Guid)fhDr["id"];
                    result.rows.Find(a => a.ID == id).AbnormalCount = aCount;
                }
            };

            return result;
        }

        /// <summary>
        /// 修改确认升级时间
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ReturnResponse EditConfirmUpgradeTime(DTOEditUpgradeInfo dto)
        {

            Repository<Prc_UpgradeInfo> repository = new Repository<Prc_UpgradeInfo>(DbContext);
            Prc_UpgradeInfo editInfo = repository.Get(a => a.ID == dto.ID);
            editInfo.ConfirmUpgradeTime = dto.ConfirmUpgradeTime;
            editInfo.Summary = dto.Summary;
            repository.Update(editInfo);
            OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "确认客户【" + editInfo.MallName + "】的升级时间为【" + dto.ConfirmUpgradeTime + "】");
            DbContext.SaveChanges();


            return new ReturnResponse();
        }

        /// <summary>
        /// 取消预约
        /// </summary>
        public ReturnResponse CancelReserve(DTOEditUpgradeInfo dto)
        {
            BllUpgrade.CancelReserve(dto.ID);

            return new ReturnResponse();
        }

        /// <summary>
        /// 取消门店预约
        /// </summary>
        public ReturnResponse CancelBusinessReserve(DTOEditUpgradeInfoItem dto)
        {
            Repository<Prc_UpgradeInfoItem> repository = new Repository<Prc_UpgradeInfoItem>(DbContext);
            Prc_UpgradeInfoItem editItem = repository.Get(a => a.ID == dto.ID);
            editItem.UpgradeStatus = EnumUpgradeStatus.Canceled;
            repository.Update(editItem);
            OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "忽略门店【" + editItem.BusinessName + "】的升级预约");
            DbContext.SaveChanges();

            return new ReturnResponse();
        }

        /// <summary>
        /// 开始升级
        /// </summary>
        /// <returns></returns>
        public ReturnResponse StartUpgrade(DTOEditUpgradeInfo dto)
        {

            //Repository<Prc_UpgradeInfoItem> repositoryItem = CreateRepository<Prc_UpgradeInfoItem>(access);
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);

            Prc_UpgradeInfo editInfo = repositoryInfo.Get(a => a.ID == dto.ID);

            editInfo.UpgradeStatus = EnumUpgradeStatus.Upgrading;
            editInfo.StartUpgradeTime = DateTime.Now;
            repositoryInfo.Update(editInfo);
            OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "启动客户【" + editInfo.MallName + "】的升级");
            DbContext.SaveChanges();


            return new ReturnResponse();
        }

        /// <summary>
        /// 查询升级信息细项
        /// </summary>
        /// <returns></returns>
        public DTOBackUpgradeInfoItem SearchUpgradeInfoItemByInfo([FromBody]DTOGetUpgradeInfoItem dto)
        {
            DTOBackUpgradeInfoItem result = new DTOBackUpgradeInfoItem();
            result.rows = new List<UpgradeInfoItem>();
            if (dto.UpgradeInfoID != Guid.Empty)
            {
                Repository<Prc_UpgradeInfoItem> repository = new Repository<Prc_UpgradeInfoItem>(DbContext);
                Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);
                Prc_UpgradeInfo info = repositoryInfo.Get(a => a.ID == dto.UpgradeInfoID && !a.IsDelete);
                if (info == null)
                {
                    throw new Exception("未找到升级信息");
                }

                bool centerHeartLost = false;
                if ((DateTime.Now - info.HeartbeatTime).TotalMinutes > 15)
                {
                    centerHeartLost = true;
                }
                List<Prc_UpgradeInfoItem> infoItems = repository.Search(a => !a.IsDelete && a.UpgradeInfo.ID == dto.UpgradeInfoID);
                infoItems.ForEach(a =>
                {
                    UpgradeInfoItem infoItem = new UpgradeInfoItem()
                    {
                        ID = a.ID,
                        BusinessType = (int)a.BusinessType,
                        MallName = a.BusinessName,
                        HeartbeatStatus = centerHeartLost ? 1 : a.HeartbeatStatus,
                        UpgradeBagStatus = (int)a.UpgradeBagStatus,
                        UpgradeStatus = (int)a.UpgradeStatus,
                        IsSingle = info.IsSingle
                    };
                    result.rows.Add(infoItem);
                });
                if (infoItems.Count > 0)
                {
                    result.InfoUpgradeStatus = (int)info.UpgradeStatus;
                }
            }
            result.rows = result.rows.OrderBy(a => a.BusinessType).ThenBy(a => a.MallName).ToList();

            return result;
        }

        /// <summary>
        /// 查询升级详情
        /// </summary>
        public DTOBackUpgradeDetail SearchUpgradeDetailByInfo([FromBody]DTOGetUpgradeDetail dto)
        {
            DTOBackUpgradeDetail result = new DTOBackUpgradeDetail() { rows = new List<UpgradeDetail>() };
            if (dto.UpgradeInfoID != Guid.Empty)
            {
              
                    Repository<Prc_UpgradeInfoItem> repository = new Repository<Prc_UpgradeInfoItem>(DbContext);

                   List<Prc_UpgradeInfoItem> infoItems = repository.Search(a => !a.IsDelete && a.UpgradeInfo.ID == dto.UpgradeInfoID);

                    Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);
                    Prc_UpgradeInfo info = repositoryInfo.Get(a => a.ID == dto.UpgradeInfoID && !a.IsDelete);

                    bool centerHeartLost = false;
                    if ((DateTime.Now - info.HeartbeatTime).TotalMinutes > 15)
                    {
                        centerHeartLost = true;
                    }
                    List<string> infoItemIds = new List<string>();

                    infoItems.ForEach(a =>
                    {
                        UpgradeDetail infoItem = new UpgradeDetail()
                        {
                            UpgradeInfoItemID = a.ID,
                            BusinessType = (int)a.BusinessType,
                            MallName = a.BusinessName,
                            HeartbeatStatus = (a.UpgradeStatus == EnumUpgradeStatus.Reserved || a.UpgradeStatus == EnumUpgradeStatus.Upgrading) ? (centerHeartLost ? 1 : a.HeartbeatStatus) : -1,
                            UpgradeStatus = (int)a.UpgradeStatus
                        };
                        if (a.UpgradeStatus == EnumUpgradeStatus.Reserved || a.UpgradeStatus == EnumUpgradeStatus.Upgrading)
                        {
                            infoItem.TimeConsum = a.StartUpgradeTime.HasValue ? (int)(DateTime.Now - a.StartUpgradeTime.GetValueOrDefault()).TotalMinutes : 0;

                        }
                        else
                        {
                            infoItem.TimeConsum = a.StartUpgradeTime.HasValue && a.EndUpgradeTime.HasValue ? (int)(a.EndUpgradeTime.GetValueOrDefault() - a.StartUpgradeTime.GetValueOrDefault()).TotalMinutes : 0;
                        }
                        infoItemIds.Add(a.ID.ToString());
                        result.rows.Add(infoItem);
                    });
                    
                    string sqlText = string.Format(@"select * from
                                            (
                                            select *, ROW_NUMBER() over(partition by UpgradeInfoItemID order by OccurTime desc) as rowNum
                                            from Prc_UpgradeMessage where UpgradeInfoItemID in ('{0}')) ranked
                                            where ranked.rowNum <= 1", string.Join("','", infoItemIds));
                    List<UpgradeDetail> msgList = repository.SearchBySql<UpgradeDetail>(sqlText);

                    result.rows.ForEach(a => {
                        var msg = msgList.Find(m => m.UpgradeInfoItemID == a.UpgradeInfoItemID);
                        if (msg != null)
                        {
                            a.ID = msg.ID;
                            a.MessageFlag = msg.MessageFlag;
                            a.Content = msg.Content;
                            a.OccurTime = msg.OccurTime;
                            a.MessageType = msg.MessageType;
                            a.HandleOptions = new List<string>();
                            a.HandleStatus = msg.HandleStatus;
                            if (!string.IsNullOrWhiteSpace(msg.MessageFlag) && msg.MessageType == (int)EnumMessageType.Question)
                            {
                                if (msg.HandleStatus == 1)
                                {
                                    if (QuestionMsgFlag.s_FlagToOperate.ContainsKey(a.MessageFlag))
                                    {
                                        QuestionMsgFlag.s_FlagToOperate[a.MessageFlag].ForEach(b =>
                                        {
                                            a.HandleOptions.Add(b.ToString());
                                        });
                                    }
                                    else
                                    {
                                        a.HandleOptions.Add(EnumQuestionResult.ReTry.ToString());
                                        a.HandleOptions.Add(EnumQuestionResult.Stop.ToString());
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(msg.Result))
                                    {
                                        if (msg.Result == EnumQuestionResult.Stop.ToString())
                                        {
                                            a.Content += " => 终止";
                                        }
                                        else if (msg.Result == EnumQuestionResult.ReTry.ToString())
                                        {
                                            a.Content += " => 重试";
                                        }
                                        else if (msg.Result == EnumQuestionResult.Ignore.ToString())
                                        {
                                            a.Content += " => 忽略";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            a.Content = string.Empty;
                        }
                    });
                
                result.rows = result.rows.OrderBy(a => a.BusinessType).ThenBy(a => a.MallName).ToList();
            }
            return result;
        }

        /// <summary>
        /// 查询升级详述
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public DTOBackUpgradeMessages SearchUpgradeMessageByItem([FromBody]DTOGetUpgradeMessages dto)
        {
            DTOBackUpgradeMessages result = new DTOBackUpgradeMessages();
            result.rows = new List<UpgradeMessage>();
            if (dto.UpgradeInfoItemID != Guid.Empty)
            {             
                    Repository<Prc_UpgradeMessage> repository = new Repository<Prc_UpgradeMessage>(DbContext);

                    List<Prc_UpgradeMessage> infoItems = repository.Search(a => !a.IsDelete && a.UpgradeInfoItem.ID == dto.UpgradeInfoItemID);
                    infoItems.ForEach(a =>
                    {
                        UpgradeMessage msg = new UpgradeMessage()
                        {
                            ID = a.ID,
                            OccurTime = a.OccurTime,
                            Content = a.Content,
                            MessageType = (int)a.MessageType
                        };
                        if (a.MessageType == EnumMessageType.Question && a.HandleStatus == EnumHandleStatus.Handled)
                        {
                            if (a.Result == EnumQuestionResult.Stop.ToString())
                            {
                                msg.Content += " => 终止";
                            }
                            else if (a.Result == EnumQuestionResult.ReTry.ToString())
                            {
                                msg.Content += " => 重试";
                            }
                            else if (a.Result == EnumQuestionResult.Ignore.ToString())
                            {
                                msg.Content += " => 忽略";
                            }
                        }
                        result.rows.Add(msg);
                    });
                }
                result.rows = result.rows.OrderByDescending(a => a.OccurTime).ToList();
            
            return result;
        }

        /// <summary>
        /// 修改处理结果
        /// </summary>
        public ReturnResponse EditHandleResult(DTOEditMessageResult dto)
        {
            Repository<Prc_UpgradeMessage> repository = new Repository<Prc_UpgradeMessage>(DbContext);
            Prc_UpgradeMessage item = repository.Get(a => a.ID == dto.ID);
            item.Result = dto.HandleResult;
            item.HandleStatus = EnumHandleStatus.Handled;
            repository.Update(item);
            DbContext.SaveChanges();
            return new ReturnResponse();
        }
    }
}
