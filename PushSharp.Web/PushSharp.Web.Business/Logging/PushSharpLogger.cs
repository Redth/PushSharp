using PILogger = PushSharp.Core.ILogger;
using NLog;

namespace PushSharp.Web.Business.Logging
{
    public class PushSharpLogger :  ApiLogger, PILogger, ILogger
    {
        public void Debug(string format, params object[] objs)
        {
            base.Debug(format);
            LogParameters(objs, LogLevel.Debug);
        }

        public void Info(string format, params object[] objs)
        {
            base.Info(format);
            LogParameters(objs, LogLevel.Info);
        }

        public void Warning(string format, params object[] objs)
        {
            base.Warn(format);
            LogParameters(objs, LogLevel.Warn);
        }

        public void Error(string format, params object[] objs)
        {
            base.Error(format);
            LogParameters(objs, LogLevel.Error);
        }

        #region Implementation

        private void LogParameters(object[] objs, LogLevel level)
        {
            var message = LogFormatter.Format(objs);
            Logger.Log(level, message);
        }

        #endregion
    }
}
