using Microsoft.EntityFrameworkCore;
using ProductMange.Model;
using ProductMange.Model.Enum;
using ProductMange.Public;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.BLL.OneKeyUpgrade
{
    public class BLLUpgrade : IBLLUpgrade
    {


        public ProductManageDbContext DbContext { get; }
        
        public BLLUpgrade(ProductManageDbContext _DbContext)
        {
            DbContext = _DbContext;
        }

        public const string SystemUser = "system";

        public Prc_VersionInfo GetNewestVersionInfo()
        {

            Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
            return repository.GetDBSet().OrderByDescending(a => a.VersionNo).FirstOrDefault(b => b.IsPublish && !b.IsDelete);

        }

        public Prc_VersionInfo GetVersionInfoById(Guid id)
        {

            Repository<Prc_VersionInfo> repository = new Repository<Prc_VersionInfo>(DbContext);
            return repository.Get(a => a.ID == id);

        }

        /// <summary>
        /// 取消预约
        /// </summary>
        /// <param name="InfoId"></param>
        public void CancelReserve(Guid InfoId)
        {

            Repository<Prc_UpgradeInfo> repository = new Repository<Prc_UpgradeInfo>(DbContext);
            Prc_UpgradeInfo editInfo = repository.Get(a => a.ID == InfoId);
            editInfo.UpgradeStatus = EnumUpgradeStatus.Canceled;
            repository.Update(editInfo);
            OperateLoger.Write(LoginUserInfo.CurrUser.UserName, DateTime.Now, "取消客户【" + editInfo.MallName + "】的升级预约");
            DbContext.SaveChanges();

        }

        public Dictionary<Guid, string> SaveMallMessage(List<Prc_UpgradeMessage> mallMsgs, Guid infoId)
        {
            Dictionary<Guid, string> failMsg = new Dictionary<Guid, string>();
            try
            {
             
                    Repository<Prc_UpgradeMessage> repositoryMsg = new Repository<Prc_UpgradeMessage>(DbContext);
                    Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(DbContext);
                    Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(DbContext);

                    Prc_UpgradeInfo info = repositoryInfo.Get(a => a.ID == infoId && !a.IsDelete);
                    if (info == null)
                    {
                        throw new Exception("未找到升级信息");
                    }

                    List<string> busNum = new List<string>();
                    mallMsgs.ForEach(a =>
                    {
                        busNum.Add(a.BusinessNum);
                    });

                    List<Prc_UpgradeInfoItem> items = repositoryItem.Search(a => a.UpgradeInfo.ID == info.ID &&
                    busNum.Contains(a.BusinessNum) && !a.IsDelete);

                    mallMsgs.ForEach(a =>
                    {
                        try
                        {
                            if (repositoryMsg.Get(b => b.ID == a.ID) == null)
                            {
                                var item = items.Find(b => b.BusinessNum == a.BusinessNum);
                                if (item == null)
                                {
                                    throw new Exception(string.Format("未找到门店{0}的升级信息", a.BusinessNum));
                                }

                                Prc_UpgradeMessage msg = new Prc_UpgradeMessage()
                                {
                                    ID = a.ID,
                                    UpgradeInfoItem = item,
                                    Content = a.Content,
                                    MessageType = (EnumMessageType)a.MessageType,
                                    MessageFlag = a.MessageFlag,
                                    OccurTime = a.OccurTime,
                                    CreateOperateUser = "system",
                                    LastUpdateTime = DateTime.Now,
                                    CreateTime = DateTime.Now,
                                    HandleStatus = EnumHandleStatus.UnHandle
                                };
                                MessageHandle(msg, DbContext);
                                repositoryMsg.Add(msg);
                            }
                        }
                        catch (Exception ex)
                        {
                            failMsg.Add(a.ID, ex.Message);
                        }
                    });
                DbContext.SaveChanges();
                
            }
            catch (Exception ex)
            {
                mallMsgs.ForEach(a =>
                {
                    if (!failMsg.ContainsKey(a.ID))
                    {
                        failMsg.Add(a.ID, ex.Message);
                    }
                });
            }
            return failMsg;
        }


        /// <summary>
        /// 消息处理
        /// </summary>
        private void MessageHandle(Prc_UpgradeMessage msg, DbContext access)
        {
            switch (msg.MessageFlag)
            {
                case InfoMsgFlag.StartDownloadUpgradeBag:
                    StartDownloadUpgradeBag(msg, access);
                    break;
                case InfoMsgFlag.FinishDownloadUpgradeBag:
                    FinishDownloadUpgradeBag(msg, access);
                    break;
                case ErrorMsgFlag.UpgradeCancel:
                    UpgradeCancel(msg, access);
                    break;
                case ErrorMsgFlag.UpgradeFail:
                    UpgradeFail(msg, access);
                    break;
                case ErrorMsgFlag.UpgradeOverdued:
                    UpgradeOverdued(msg, access);
                    break;
                case InfoMsgFlag.StartUpgrade:
                    StartUpgrade(msg, access);
                    break;
                case InfoMsgFlag.UpgradeSucc:
                    UpgradeSucc(msg, access);
                    break;
                case InfoMsgFlag.DataSynTaskWait:
                case InfoMsgFlag.DataSynTaskFinish:
                    DataSynTaskHandle(msg, access);
                    break;
            }
        }

        private void DataSynTaskHandle(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeMessage> repositoryMsg = new Repository<Prc_UpgradeMessage>(access);
            var delMsg = repositoryMsg.Get(a => a.UpgradeInfoItem.ID == msg.UpgradeInfoItem.ID && a.MessageFlag == InfoMsgFlag.DataSynTaskWait);
            if (delMsg != null)
            {
                repositoryMsg.Delete(delMsg);
            }

        }

        /// <summary>
        /// 升级成功
        /// </summary>
        private void UpgradeSucc(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            var item = repositoryItem.Get(a => a.ID == msg.UpgradeInfoItem.ID);
            item.UpgradeStatus = EnumUpgradeStatus.UpgradeSucc;
            item.EndUpgradeTime = msg.OccurTime;
            repositoryItem.Update(item);
            CheckFinish(msg, access);
        }

        /// <summary>
        /// 开始升级
        /// </summary>
        private void StartUpgrade(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            var item = repositoryItem.Get(a => a.ID == msg.UpgradeInfoItem.ID);
            if (item.UpgradeStatus == EnumUpgradeStatus.Reserved)
            {
                item.StartUpgradeTime = msg.OccurTime;
                item.UpgradeStatus = EnumUpgradeStatus.Upgrading;
                repositoryItem.Update(item);
            }
        }

        /// <summary>
        /// 开始下载
        /// </summary>
        private void StartDownloadUpgradeBag(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            var item = repositoryItem.Get(a => a.ID == msg.UpgradeInfoItem.ID);
            if (item.UpgradeBagStatus == EnumUpgradeBagStatus.None)
            {
                item.UpgradeBagStatus = EnumUpgradeBagStatus.Downloading;
                repositoryItem.Update(item);
            }
        }

        /// <summary>
        /// 完成下载
        /// </summary>
        private void FinishDownloadUpgradeBag(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            var item = repositoryItem.Get(a => a.ID == msg.UpgradeInfoItem.ID);
            item.UpgradeBagStatus = EnumUpgradeBagStatus.Downloaded;
            repositoryItem.Update(item);
        }

        /// <summary>
        /// 取消升级
        /// </summary>
        private void UpgradeCancel(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeMessage> repositoryMsg = new Repository<Prc_UpgradeMessage>(access);
            var delMsg = repositoryMsg.Get(a => a.UpgradeInfoItem.ID == msg.UpgradeInfoItem.ID && a.MessageFlag == ErrorMsgFlag.UpgradeCancel);
            if (delMsg != null)
            {
                repositoryMsg.Delete(delMsg);
            }

            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            var item = repositoryItem.Get(a => a.ID == msg.UpgradeInfoItem.ID);
            item.UpgradeStatus = EnumUpgradeStatus.Canceled;
            repositoryItem.Update(item);
            CheckFinish(msg, access);
        }

        /// <summary>
        /// 升级失败
        /// </summary>
        private void UpgradeFail(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(access);

            var item = repositoryItem.GetDBSet().Include(o => o.UpgradeInfo).First(a => a.ID == msg.UpgradeInfoItem.ID);
            item.UpgradeStatus = EnumUpgradeStatus.UpgradeFail;
            item.EndUpgradeTime = msg.OccurTime;
            if (item.BusinessType == EnumBusinessType.Center || item.UpgradeInfo.IsSingle)
            {
                //更新主项的状态
                item.UpgradeInfo.UpgradeStatus = EnumUpgradeStatus.UpgradeFail;
                item.UpgradeInfo.EndUpgradeTime = msg.OccurTime;
                repositoryInfo.Update(item.UpgradeInfo);
            }
            repositoryItem.Update(item);
            CheckFinish(msg, DbContext);

        }

        /// <summary>
        /// 升级过期
        /// </summary>
        private void UpgradeOverdued(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeMessage> repositoryMsg = new Repository<Prc_UpgradeMessage>(access);
            var delMsg = repositoryMsg.Get(a => a.UpgradeInfoItem.ID == msg.UpgradeInfoItem.ID && a.MessageFlag == ErrorMsgFlag.UpgradeOverdued);
            if (delMsg != null)
            {
                repositoryMsg.Delete(delMsg);
            }

            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(access);

            var item = repositoryItem.GetDBSet().Include(o => o.UpgradeInfo).First(a => a.ID == msg.UpgradeInfoItem.ID);
            item.UpgradeStatus = EnumUpgradeStatus.Canceled;
            item.EndUpgradeTime = msg.OccurTime;
            if (item.BusinessType == EnumBusinessType.Center || item.UpgradeInfo.IsSingle)
            {
                //更新主项的状态
                item.UpgradeInfo.UpgradeStatus = EnumUpgradeStatus.Overdued;
                item.UpgradeInfo.EndUpgradeTime = msg.OccurTime;
                repositoryInfo.Update(item.UpgradeInfo);
            }
            repositoryItem.Update(item);
            CheckFinish(msg, access);

        }

        /// <summary>
        /// 是否已升级完成
        /// </summary>
        private void CheckFinish(Prc_UpgradeMessage msg, DbContext access)
        {
            Repository<Prc_UpgradeInfoItem> repositoryItem = new Repository<Prc_UpgradeInfoItem>(access);
            Repository<Prc_UpgradeInfo> repositoryInfo = new Repository<Prc_UpgradeInfo>(access);
            access.SaveChanges();
            var item = repositoryItem.GetDBSet().Include(o => o.UpgradeInfo).First(a => a.ID == msg.UpgradeInfoItem.ID);
            if (item.UpgradeInfo.UpgradeStatus == EnumUpgradeStatus.Upgrading || item.UpgradeInfo.UpgradeStatus == EnumUpgradeStatus.Reserved)
            {
                var unFinish = repositoryItem.Get(a => (a.UpgradeStatus == EnumUpgradeStatus.Reserved || a.UpgradeStatus == EnumUpgradeStatus.Upgrading) &&
                a.UpgradeInfo.ID == item.UpgradeInfo.ID);
                if (unFinish == null)
                {
                    var failBus = repositoryItem.Get(a => a.UpgradeStatus == EnumUpgradeStatus.UpgradeFail &&
                                                        a.UpgradeInfo.ID == item.UpgradeInfo.ID);
                    if (failBus == null)
                    {
                        item.UpgradeInfo.UpgradeStatus = EnumUpgradeStatus.UpgradeSucc;
                    }
                    else
                    {
                        item.UpgradeInfo.UpgradeStatus = EnumUpgradeStatus.UpgradeFail;
                    }
                    item.UpgradeInfo.EndUpgradeTime = msg.OccurTime;
                    repositoryInfo.Update(item.UpgradeInfo);
                }
            }
        }
    }
}
