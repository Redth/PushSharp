using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace PushSharp.Core
{
	public class Log
	{
		private static NLog.Logger Logger = NLog.LogManager.GetLogger("PushSharpLog");

		public static void Debug(string format, params object[] objs)
		{
			Logger.Debug(format, objs);
		}

		public static void Info(string format, params object[] objs)
		{
			Logger.Info(format, objs); 
		}

		public static void Warning(string format, params object[] objs)
		{
			Logger.Warn(format, objs);
		}

		public static void Error(string format, params object[] objs)
		{
			Logger.Error(format, objs); 

		}
		public static void Error(string format, Exception ex, params object[] objs)
		{
			Logger.ErrorException(string.Format(format, objs), ex);
		}

	}
}
