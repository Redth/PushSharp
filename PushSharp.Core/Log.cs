using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PushSharp.Core
{
	public class Log
	{
		public static LogLevel Level = LogLevel.None;

		public static void Info(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Info))
				Console.WriteLine("INFO [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
		}

		public static void Warning(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Warning))
				Console.WriteLine("WARN [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
		}

		public static void Error(string format, params object[] objs)
		{
			if (((int)Level) >= ((int)LogLevel.Error))
				Console.WriteLine("ERR  [" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + format, objs);
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
