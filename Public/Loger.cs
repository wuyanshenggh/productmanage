using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMange 
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Loger
    {
        string LocalLogDir;

        public Loger()
        {
            LocalLogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            try
            {
                System.IO.Directory.CreateDirectory(LocalLogDir);
            }
            catch
            {
            }
        }

        public Loger(string dir, string catalog)
        {
            _Catelog = catalog;
            LocalLogDir = dir;
            try
            {
                System.IO.Directory.CreateDirectory(LocalLogDir);
            }
            catch
            {
            }
        }

        public Loger(string catalog)
        {
            _Catelog = catalog;
            LocalLogDir = AppDomain.CurrentDomain.BaseDirectory + "\\Log";
            try
            {
                System.IO.Directory.CreateDirectory(LocalLogDir);
            }
            catch
            {
            }
        }

        string _Catelog;

        public string Catelog
        {
            get { return _Catelog; }
            set { _Catelog = value; }
        }


        private void LogInfo(string msg, string FromModel, string StackTrace, string Errlevel, Exception ex)
        {
            lock (this)
            {
                string Logfile = string.Format("{0}\\{1}{2:yyyy-MM-dd}.log", LocalLogDir, Catelog, System.DateTime.Now);
                string logContent = "";
                if (Errlevel == "Error" || Errlevel == "Debug")
                {
                    logContent = string.Format("LogInfo:{0},{1},{2}\r\n{3}\r\nStackTrace:\r\n{4}\r\n\r\n", System.DateTime.Now, Errlevel, msg, ex, StackTrace);
                }
                else
                {
                    try
                    {
                        logContent = string.Format("Log:{0},{1},{2}\r\n{3}\r\n\r\n", System.DateTime.Now, Errlevel, msg, ex);
                    }
                    catch
                    {
                        try
                        {

                            logContent = string.Format("Log:{0},{1},{2}\r\n{3}\r\n\r\n", System.DateTime.Now, Errlevel, msg, ex.Message);
                        }
                        catch
                        {
                            logContent = "UnKnow error";
                        }
                    }
                }
                try
                {
                    System.IO.File.AppendAllText(Logfile, logContent);
                }
                catch
                {
                }

            }
        }

        public void Info(string msg)
        {
            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, null, "Info", null);
        }



        public void Error(string msg, Exception ex)
        {
//#if DEBUG
            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, Environment.StackTrace, "Error", ex);
//#else
//            LogInfo(msg, System.Reflection.Assembly.GetCallingAssembly().FullName, null, "Error", ex);
//#endif
        }
    }
}
