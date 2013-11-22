using System.Web.Mvc;
using PushSharp.Web.Business.Logging;

namespace PushSharp.Web.Business
{
    public class PushSharpHandleErrorAttribute : HandleErrorAttribute
    {
        private static  ILogger _logger;

        public override void OnException(ExceptionContext filterContext)
        {
            var logger = GetLogger();
            logger.Error(filterContext.Exception);
        }

        private ILogger GetLogger()
        {
            return _logger ?? (_logger = new ApiLogger());
        }
    }
}
