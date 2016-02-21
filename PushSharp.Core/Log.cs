using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PushSharp.Core
{
    [Flags]
    public enum LogLevel
    {
        Info = 0,
        Debug = 2,
        Error = 8
    }

    public interface ILogger
    {
        void Write (LogLevel level, string msg, params object[] args);
    }

    public static class Log
    {
        static readonly object loggerLock = new object ();

        static List<ILogger> loggers { get; set; }
        static Dictionary<CounterToken, Stopwatch> counters;

        static Log ()
        {
            counters = new Dictionary<CounterToken, Stopwatch> ();
            loggers = new List<ILogger> ();

            AddLogger (new ConsoleLogger ());
        }

        public static void AddLogger (ILogger logger)
        {
            lock (loggerLock)
                loggers.Add (logger);
        }

        public static void ClearLoggers ()
        {
            lock (loggerLock)
                loggers.Clear ();
        }

        public static IEnumerable<ILogger> Loggers {
            get { return loggers; }
        }

        public static void Write (LogLevel level, string msg, params object[] args)
        {
            lock (loggers) {
                foreach (var l in loggers)
                    l.Write (level, msg, args);
            }
        }

        public static void Info (string msg, params object[] args)
        {
            Write (LogLevel.Info, msg, args);
        }

        public static void Debug (string msg, params object[] args)
        {
            Write (LogLevel.Debug, msg, args);
        }

        public static void Error (string msg, params object[] args)
        {
            Write (LogLevel.Error, msg, args);
        }

        public static CounterToken StartCounter ()
        {
            var t = new CounterToken {
                Id = Guid.NewGuid ().ToString ()
            };

            var sw = new Stopwatch ();

            counters.Add (t, sw);

            sw.Start ();

            return t;
        }

        public static TimeSpan StopCounter (CounterToken counterToken)
        {
            if (!counters.ContainsKey (counterToken))
                return TimeSpan.Zero;

            var sw = counters [counterToken];

            sw.Stop ();

            counters.Remove (counterToken);

            return sw.Elapsed;
        }

        public static void StopCounterAndLog (CounterToken counterToken, string msg, LogLevel level = LogLevel.Info)
        {
            var elapsed = StopCounter (counterToken);

            if (!msg.Contains ("{0}"))
                msg += " {0}";

            Log.Write (level, msg, elapsed.TotalMilliseconds);
        }
    }

    public static class CounterExtensions
    {
        public static void StopAndLog (this CounterToken counterToken, string msg, LogLevel level = LogLevel.Info)
        {
            Log.StopCounterAndLog (counterToken, msg, level);
        }

        public static TimeSpan Stop (this CounterToken counterToken)
        {
            return Log.StopCounter (counterToken);
        }
    }

    public class CounterToken
    {
        public string Id { get;set; }
    }

    public class ConsoleLogger : ILogger
    {
        public void Write (LogLevel level, string msg, params object[] args)
        {
            var s = msg;

            if (args != null && args.Length > 0)
                s = string.Format (msg, args);

            var d = DateTime.Now.ToString ("yyyy-MM-dd HH:mm:ss.ttt");

            switch (level) {
            case LogLevel.Info:
                Console.Out.WriteLine (d + " [INFO] " + s);
                break;
            case LogLevel.Debug:
                Console.Out.WriteLine (d + " [DEBUG] " + s);
                break;
            case LogLevel.Error:
                Console.Error.WriteLine (d + " [ERROR] " + s);
                break;
            }
        }
    }
}

