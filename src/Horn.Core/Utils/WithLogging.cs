using log4net;
namespace Horn.Core.Utils
{
    public class WithLogging
    {

        private ILog log;


        protected WithLogging()
        {
            log = LogManager.GetLogger(GetType());
        }

        protected void InfoFormat(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        protected void Info(object message)
        {
            log.Info(message);
        }



    }
}