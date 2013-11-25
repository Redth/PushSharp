using System;
using NLog;

namespace PushSharp.Web.Business.Logging
{
    public class ApiLogger : ILogger
    {
        protected readonly Logger Logger;

        public ApiLogger()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Warn(string message)
        {
            Logger.Warn(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Error(Exception x)
        {
            Error(LogFormatter.BuildExceptionMessage(x));
        }

        public void Error(string message, Exception x)
        {
            Logger.ErrorException(message, x);
        }

        public void Fatal(string message)
        {
            Logger.Fatal(message);
        }

        public void Fatal(Exception x)
        {
            Fatal(LogFormatter.BuildExceptionMessage(x));
        }
    }
}