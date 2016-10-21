using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLInfoCollectionService.Contracts;
using log4net;

namespace SQLInfoCollectionService
{
    public class Logger : ILogger
    {
        private readonly log4net.ILog log =
               log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Logger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        public void Debug(string msg)
        {
            log.Debug(msg);
        }

        public void Debug(string msg, Exception exception)
        {
            log.Debug(msg, exception);
        }

        public void Info(string msg)
        {
            log.Info(msg);
        }

        public void Info(string msg, Exception exception)
        {
            log.Info(msg, exception);
        }

        public void Error(string msg)
        {
            log.Error(msg);
        }

        public void Error(string msg, Exception exception)
        {
            log.Error(msg, exception);
        }
    }
}
