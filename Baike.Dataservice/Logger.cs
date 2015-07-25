using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log4Net.Config", Watch = true)]
namespace Log.service
{
    public class Logger
    {
        #region fields

        private static ILog _exceptionLogger = LogManager.GetLogger(LoggerType.ExceptionLogger.ToString());
        private static ILog _infoLogger = LogManager.GetLogger(LoggerType.InfoLogger.ToString());

        #endregion

        #region Methods

        public static void Error(object error)
        {
            _exceptionLogger.Error(error);
        }

        public static void Info(object error)
        {
            _infoLogger.Info(error);
        }

        #endregion
    }


    public enum LoggerType
    {
        ExceptionLogger = 0,
        InfoLogger = 1
    }
}
