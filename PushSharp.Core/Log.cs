using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public class Log
	{
		public static ILogger Logger { get;set; }

		static Log()
		{
			Logger = null;
		}
		public static LogLevel Level = LogLevel.None;

		public static void Debug(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Info))
			{
				if (Logger == null)
					Console.WriteLine ("DEBUG [" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				else
					Logger.Debug (format, objs);
			}
		}

		public static void Info(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Info))
			{
				if (Logger == null)
					Console.WriteLine ("INFO [" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				else
					Logger.Info (format, objs);
			}
		}

		public static void Warning(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Warning))
			{
				if (Logger == null)
					Console.WriteLine ("WARN [" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				else
					Logger.Warning (format, objs);
			}
		}

		public static void Error(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Error))
			{
				if (Logger == null)
					Console.WriteLine ("ERR  [" + DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
				else
					Logger.Error (format, objs);
			}
		}

	}

	public enum LogLevel
	{
		None = 0,
		Warning = 1,
		Error = 2,
		Info = 3,
		Debug = 4
	}
}
