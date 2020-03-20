using System;
using System.Collections.Generic;
using ProductMange.Model;

namespace ProductMange.BLL.OneKeyUpgrade
{
    public interface IBLLUpgrade
    {
        void CancelReserve(Guid InfoId);
        Prc_VersionInfo GetNewestVersionInfo();
        Prc_VersionInfo GetVersionInfoById(Guid id);
        Dictionary<Guid, string> SaveMallMessage(List<Prc_UpgradeMessage> mallMsgs, Guid infoId);
    }
}