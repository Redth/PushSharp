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

		public static LogLevel Level = LogLevel.Info;

		public static void Info(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Info))
				//Console.WriteLine("INFO [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				Logger.Info(format, objs); 
		}

		public static void Warning(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Warning))
				//Console.WriteLine("WARN [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				Logger.Warn(format, objs);
		}

		public static void Error(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Error))
				//Console.WriteLine("ERR  [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				Logger.Error(format, objs); 

		}
		public static void Error(string format, Exception ex, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Error))
				//Console.WriteLine("ERR  [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				Logger.ErrorException(string.Format(format, objs), ex);
		}

	}

	public enum LogLevel
	{
		None = 0,
		Warning = 1,
		Error = 2,
		Info = 3
	}
}
