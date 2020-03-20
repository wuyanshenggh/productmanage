using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange.Public
{
    public class OperateLoger
    {
        static Loger logger = new Loger("OperateLog");

        public static void Write(string operater,DateTime operateTime,string context)
        {
            string logText = string.Format("用户【{0}】于【{1}】{2}", operater, operateTime, context);
            logger.Info(logText);
        }
    }
}
