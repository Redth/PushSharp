using System;
using System.Linq;
using System.Web;

namespace PushSharp.Web.Business.Logging
{
    public class LogFormatter
    {
        public static string BuildExceptionMessage(Exception x)
        {

            Exception logException = x;
            if (x.InnerException != null)
            {
                logException = x.InnerException;
            }

            string strErrorMsg = Environment.NewLine + "Error in Path :" + HttpContext.Current.Request.Path;

            // Get the QueryString along with the Virtual Path
            strErrorMsg += Environment.NewLine + "Raw Url :" + HttpContext.Current.Request.RawUrl;

            // Get the error message
            strErrorMsg += Environment.NewLine + "Message :" + logException.Message;

            // Source of the message
            strErrorMsg += Environment.NewLine + "Source :" + logException.Source;

            // Stack Trace of the error

            strErrorMsg += Environment.NewLine + "Stack Trace :" + logException.StackTrace;

            // Method where the error occurred
            strErrorMsg += Environment.NewLine + "TargetSite :" + logException.TargetSite;
            
            return strErrorMsg;
        }

        public static string Format(params object[] objects)
        {
            var message = string.Empty;
            if (objects != null && objects.Any())
            {
                foreach (var param in objects)
                {
                    if (param != null)
                    {
                        message += param is Exception ? BuildExceptionMessage((Exception) param) : param.ToString();
                    }
                }
            }
            return message;
        }
    }
}