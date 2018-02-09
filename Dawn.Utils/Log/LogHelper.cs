using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dawn.Utils.Log
{
    public static class LogHelper
    {
        private static ILog _log = null;

        private static object _loadLock = new object();

        private static string _LOGNAME = string.Empty;

        private static Dictionary<string, ILog> _logDict = new Dictionary<string, ILog>();

        private static ILog ILogInstance
        {
            get
            {
                if (LogHelper._log == null)
                {
                    lock (LogHelper._loadLock)
                    {
                        if (LogHelper._log == null)
                        {
                            string logName = ConfigurationManager.AppSettings["appName"];
                            if (String.IsNullOrEmpty(logName))
                            {
                                logName = "log";
                            }
                            LogHelper._LOGNAME = logName;
                            XmlConfigurator.Configure();
                            LogHelper._log = LogManager.GetLogger(logName);
                        }
                    }
                }
                return LogHelper._log;
            }
        }

        public static ILog GetILogInstance()
        {
            return LogHelper.ILogInstance;
        }

        public static ILog GetILogInstance(string appName)
        {
            if (string.IsNullOrEmpty(appName))
            {
                return LogHelper.ILogInstance;
            }
            if (LogHelper._logDict.ContainsKey(appName))
            {
                return LogHelper._logDict[appName];
            }
            ILog logger = LogManager.GetLogger(appName);
            if (logger != null)
            {
                LogHelper._logDict[appName] = logger;
            }
            return logger;
        }
    }
}
