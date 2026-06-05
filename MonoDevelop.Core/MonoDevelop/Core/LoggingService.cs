using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Mono.Addins;
using Mono.Unix.Native;
using MonoDevelop.Core.Logging;
using MonoDevelop.Core.LogReporting;
using MonoDevelop.Core.ProgressMonitoring;

namespace MonoDevelop.Core
{
	// Token: 0x02000054 RID: 84
	public static class LoggingService
	{
		// Token: 0x0600028D RID: 653 RVA: 0x0000A1A8 File Offset: 0x000083A8
		static LoggingService()
		{
			ConsoleLogger consoleLogger = new ConsoleLogger();
			LoggingService.loggers.Add(consoleLogger);
			LoggingService.loggers.Add(new InstrumentationLogger());
			string environmentVariable = Environment.GetEnvironmentVariable("MONODEVELOP_CONSOLE_LOG_LEVEL");
			if (!string.IsNullOrEmpty(environmentVariable))
			{
				try
				{
					consoleLogger.EnabledLevel = (EnabledLoggingLevel)Enum.Parse(typeof(EnabledLoggingLevel), environmentVariable, true);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error setting log level", ex);
				}
			}
			string environmentVariable2 = Environment.GetEnvironmentVariable("MONODEVELOP_CONSOLE_LOG_USE_COLOUR");
			if (!string.IsNullOrEmpty(environmentVariable2) && environmentVariable2.ToLower() == "false")
			{
				consoleLogger.UseColour = false;
			}
			else
			{
				consoleLogger.UseColour = true;
			}
			string environmentVariable3 = Environment.GetEnvironmentVariable("MONODEVELOP_LOG_FILE");
			if (!string.IsNullOrEmpty(environmentVariable3))
			{
				try
				{
					FileLogger fileLogger = new FileLogger(environmentVariable3);
					LoggingService.loggers.Add(fileLogger);
					string environmentVariable4 = Environment.GetEnvironmentVariable("MONODEVELOP_FILE_LOG_LEVEL");
					fileLogger.EnabledLevel = (EnabledLoggingLevel)Enum.Parse(typeof(EnabledLoggingLevel), environmentVariable4, true);
				}
				catch (Exception ex2)
				{
					LoggingService.LogError("Error setting custom log file", ex2);
				}
			}
			try
			{
				LoggingService.PurgeOldLogs();
			}
			catch
			{
				LoggingService.LogError("Could not purge old log files");
			}
			LoggingService.timestamp = DateTime.Now;
			Debug.Listeners.Clear();
			Debug.Listeners.Add(new AssertLoggingTraceListener());
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000A324 File Offset: 0x00008524
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000A330 File Offset: 0x00008530
		public static bool? ReportCrashes
		{
			get
			{
				return PropertyService.Get<bool?>("MonoDevelop.LogAgent.ReportCrashes");
			}
			set
			{
				PropertyService.Set("MonoDevelop.LogAgent.ReportCrashes", value);
			}
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000290 RID: 656 RVA: 0x0000A342 File Offset: 0x00008542
		// (remove) Token: 0x06000291 RID: 657 RVA: 0x0000A34F File Offset: 0x0000854F
		public static event EventHandler<PropertyChangedEventArgs> ReportCrashesChanged
		{
			add
			{
				PropertyService.AddPropertyHandler("MonoDevelop.LogAgent.ReportCrashes", value);
			}
			remove
			{
				PropertyService.RemovePropertyHandler("MonoDevelop.LogAgent.ReportCrashes", value);
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000A35C File Offset: 0x0000855C
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000A368 File Offset: 0x00008568
		public static bool? ReportUsage
		{
			get
			{
				return PropertyService.Get<bool?>("MonoDevelop.LogAgent.ReportUsage");
			}
			set
			{
				PropertyService.Set("MonoDevelop.LogAgent.ReportUsage", value);
			}
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000294 RID: 660 RVA: 0x0000A37A File Offset: 0x0000857A
		// (remove) Token: 0x06000295 RID: 661 RVA: 0x0000A387 File Offset: 0x00008587
		public static event EventHandler<PropertyChangedEventArgs> ReportUsageChanged
		{
			add
			{
				PropertyService.AddPropertyHandler("MonoDevelop.LogAgent.ReportUsage", value);
			}
			remove
			{
				PropertyService.RemovePropertyHandler("MonoDevelop.LogAgent.ReportUsage", value);
			}
		}

		/// <summary>
		/// Creates a session log file with the given identifier.
		/// </summary>
		/// <returns>A TextWriter, null if the file cannot be created.</returns>
		// Token: 0x06000296 RID: 662 RVA: 0x0000A394 File Offset: 0x00008594
		public static TextWriter CreateLogFile(string identifier)
		{
			string text;
			return LoggingService.CreateLogFile(identifier, out text);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A3AC File Offset: 0x000085AC
		public static TextWriter CreateLogFile(string identifier, out string filename)
		{
			FilePath logDir = UserProfile.Current.LogDir;
			Directory.CreateDirectory(logDir);
			int num = LoggingService.logFileSuffix;
			TextWriter result;
			for (;;)
			{
				filename = logDir.Combine(new string[]
				{
					LoggingService.GetSessionLogFileName(identifier)
				});
				try
				{
					FileStream stream = File.Open(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
					result = new StreamWriter(stream)
					{
						AutoFlush = true
					};
				}
				catch (Exception ex)
				{
					if (LoggingService.logFileSuffix < num + 10)
					{
						int num2 = Marshal.GetHRForException(ex) & 65535;
						if (num2 == 80 || num2 == 32)
						{
							LoggingService.logFileSuffix++;
							continue;
						}
					}
					LoggingService.LogInternalError("Failed to create log file.", ex);
					result = null;
				}
				break;
			}
			return result;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A470 File Offset: 0x00008670
		private static string GetSessionLogFileName(string logName)
		{
			if (LoggingService.logFileSuffix == 0)
			{
				return string.Format("{0}.{1}.log", logName, LoggingService.timestamp.ToString("yyyy-MM-dd__HH-mm-ss"));
			}
			return string.Format("{0}.{1}-{2}.log", logName, LoggingService.timestamp.ToString("yyyy-MM-dd__HH-mm-ss"), LoggingService.logFileSuffix);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A4C3 File Offset: 0x000086C3
		public static void Initialize(bool redirectOutput)
		{
			if (Platform.IsWindows || redirectOutput)
			{
				LoggingService.RedirectOutputToLogFile();
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000A4D4 File Offset: 0x000086D4
		public static void Shutdown()
		{
			LoggingService.RestoreOutputRedirection();
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000A4DC File Offset: 0x000086DC
		public static void RegisterCrashReporter(CrashReporter reporter)
		{
			lock (LoggingService.customCrashReporters)
			{
				LoggingService.customCrashReporters.Add(reporter);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000A520 File Offset: 0x00008720
		public static void UnregisterCrashReporter(CrashReporter reporter)
		{
			lock (LoggingService.customCrashReporters)
			{
				LoggingService.customCrashReporters.Remove(reporter);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000A568 File Offset: 0x00008768
		internal static void ReportUnhandledException(Exception ex, bool willShutDown)
		{
			LoggingService.ReportUnhandledException(ex, willShutDown, false, null);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000A573 File Offset: 0x00008773
		internal static void ReportUnhandledException(Exception ex, bool willShutDown, bool silently)
		{
			LoggingService.ReportUnhandledException(ex, willShutDown, silently, null);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000A580 File Offset: 0x00008780
		internal static void ReportUnhandledException(Exception ex, bool willShutDown, bool silently, string tag)
		{
			try
			{
				List<string> tags = new List<string>
				{
					tag
				};
				if (!LoggingService.reporting)
				{
					LoggingService.reporting = true;
					bool? reportCrashes = LoggingService.ReportCrashes;
					if (LoggingService.UnhandledErrorOccured != null && !silently)
					{
						LoggingService.ReportCrashes = LoggingService.UnhandledErrorOccured(LoggingService.ReportCrashes, ex, willShutDown);
					}
					if (LoggingService.ReportCrashes == null || LoggingService.ReportCrashes.Value)
					{
						lock (LoggingService.customCrashReporters)
						{
							foreach (CrashReporter crashReporter in LoggingService.customCrashReporters.Concat(AddinManager.GetExtensionObjects<CrashReporter>(true)))
							{
								crashReporter.ReportCrash(ex, willShutDown, tags);
							}
						}
						if (LoggingService.ReportCrashes != reportCrashes)
						{
							PropertyService.SaveProperties();
						}
					}
				}
			}
			catch
			{
			}
			finally
			{
				LoggingService.reporting = false;
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000A740 File Offset: 0x00008940
		private static void PurgeOldLogs()
		{
			if (!Directory.Exists(UserProfile.Current.LogDir))
			{
				return;
			}
			IEnumerable<FileInfo> enumerable = from f in Directory.GetFiles(UserProfile.Current.LogDir)
			select new FileInfo(f) into f
			where f.CreationTimeUtc < DateTime.UtcNow.Subtract(TimeSpan.FromDays(7.0))
			select f;
			foreach (FileInfo fileInfo in enumerable)
			{
				try
				{
					fileInfo.Delete();
				}
				catch (Exception value)
				{
					Console.Error.WriteLine(value);
				}
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000A814 File Offset: 0x00008A14
		private static void RedirectOutputToLogFile()
		{
			try
			{
				if (Platform.IsWindows)
				{
					LoggingService.RedirectOutputToFileWindows();
				}
				else
				{
					LoggingService.RedirectOutputToFileUnix();
				}
			}
			catch (Exception ex)
			{
				LoggingService.LogInternalError("Failed to redirect output to log file", ex);
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000A854 File Offset: 0x00008A54
		private static void RedirectOutputToFileWindows()
		{
			LoggingService.writer = LoggingService.CreateLogFile("Ide");
			if (LoggingService.writer == Console.Out)
			{
				return;
			}
			LoggingService.stderr = new LogTextWriter();
			LoggingService.stderr.ChainWriter(Console.Error);
			LoggingService.stderr.ChainWriter(LoggingService.writer);
			LoggingService.defaultError = Console.Error;
			Console.SetError(LoggingService.stderr);
			LoggingService.stdout = new LogTextWriter();
			LoggingService.stdout.ChainWriter(Console.Out);
			LoggingService.stdout.ChainWriter(LoggingService.writer);
			LoggingService.defaultOut = Console.Out;
			Console.SetOut(LoggingService.stdout);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000A8F8 File Offset: 0x00008AF8
		private static void RedirectOutputToFileUnix()
		{
			FilePath logDir = UserProfile.Current.LogDir;
			Directory.CreateDirectory(logDir);
			int num = LoggingService.logFileSuffix;
			Errno lastError;
			for (;;)
			{
				string text = logDir.Combine(new string[]
				{
					LoggingService.GetSessionLogFileName("Ide")
				});
				int num2 = Syscall.open(text, 577, 33200);
				if (num2 >= 0)
				{
					goto IL_94;
				}
				lastError = Stdlib.GetLastError();
				if (LoggingService.logFileSuffix < num + 10 && lastError == 17)
				{
					break;
				}
				LoggingService.logFileSuffix++;
			}
			throw new IOException("Unable to open file: " + lastError);
			try
			{
				IL_94:
				int num2;
				int num3 = Syscall.dup2(num2, 1);
				if (num3 < 0)
				{
					throw new IOException("Unable to redirect stdout: " + Stdlib.GetLastError());
				}
				num3 = Syscall.dup2(num2, 2);
				if (num3 < 0)
				{
					throw new IOException("Unable to redirect stderr: " + Stdlib.GetLastError());
				}
				string text;
				LoggingService.SymlinkWithRetry(text, logDir.Combine(new string[]
				{
					"Ide.log"
				}), 10);
			}
			finally
			{
				int num2;
				Syscall.close(num2);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000AA2C File Offset: 0x00008C2C
		private static bool SymlinkWithRetry(string from, string to, int retries)
		{
			for (int i = 0; i < retries; i++)
			{
				Syscall.unlink(to);
				if (Syscall.symlink(from, to) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000AA59 File Offset: 0x00008C59
		private static void RestoreOutputRedirection()
		{
			if (LoggingService.defaultError != null)
			{
				Console.SetError(LoggingService.defaultError);
			}
			if (LoggingService.defaultOut != null)
			{
				Console.SetOut(LoggingService.defaultOut);
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000AA7D File Offset: 0x00008C7D
		internal static RemoteLogger RemoteLogger
		{
			get
			{
				if (LoggingService.remoteLogger == null)
				{
					LoggingService.remoteLogger = new RemoteLogger();
				}
				return LoggingService.remoteLogger;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000AA98 File Offset: 0x00008C98
		public static bool IsLevelEnabled(LogLevel level)
		{
			foreach (ILogger logger in LoggingService.loggers)
			{
				if ((logger.EnabledLevel & (EnabledLoggingLevel)level) == (EnabledLoggingLevel)level)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000AAF8 File Offset: 0x00008CF8
		public static void Log(LogLevel level, string message)
		{
			foreach (ILogger logger in LoggingService.loggers)
			{
				if ((logger.EnabledLevel & (EnabledLoggingLevel)level) == (EnabledLoggingLevel)level)
				{
					logger.Log(level, message);
				}
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000AB58 File Offset: 0x00008D58
		public static ILogger GetLogger(string name)
		{
			foreach (ILogger logger in LoggingService.loggers)
			{
				if (logger.Name == name)
				{
					return logger;
				}
			}
			return null;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000ABB8 File Offset: 0x00008DB8
		public static void AddLogger(ILogger logger)
		{
			if (LoggingService.GetLogger(logger.Name) != null)
			{
				throw new Exception("There is already a logger with the name '" + logger.Name + "'");
			}
			LoggingService.loggers.Add(logger);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000ABF0 File Offset: 0x00008DF0
		public static void RemoveLogger(string name)
		{
			ILogger logger = LoggingService.GetLogger(name);
			if (logger == null)
			{
				throw new Exception("There is no logger registered with the name '" + name + "'");
			}
			LoggingService.loggers.Remove(logger);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000AC29 File Offset: 0x00008E29
		public static void LogDebug(string message)
		{
			LoggingService.Log(LogLevel.Debug, message);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000AC33 File Offset: 0x00008E33
		public static void LogInfo(string message)
		{
			LoggingService.Log(LogLevel.Info, message);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000AC3C File Offset: 0x00008E3C
		public static void LogWarning(string message)
		{
			LoggingService.Log(LogLevel.Warn, message);
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000AC45 File Offset: 0x00008E45
		public static void LogError(string message)
		{
			LoggingService.Log(LogLevel.Error, message);
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000AC4E File Offset: 0x00008E4E
		public static void LogFatalError(string message)
		{
			LoggingService.Log(LogLevel.Fatal, message);
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000AC57 File Offset: 0x00008E57
		public static void LogDebug(string messageFormat, params object[] args)
		{
			LoggingService.Log(LogLevel.Debug, string.Format(messageFormat, args));
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000AC67 File Offset: 0x00008E67
		public static void LogInfo(string messageFormat, params object[] args)
		{
			LoggingService.Log(LogLevel.Info, string.Format(messageFormat, args));
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000AC76 File Offset: 0x00008E76
		public static void LogWarning(string messageFormat, params object[] args)
		{
			LoggingService.Log(LogLevel.Warn, string.Format(messageFormat, args));
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000AC85 File Offset: 0x00008E85
		public static void LogUserError(string messageFormat, params object[] args)
		{
			LoggingService.Log(LogLevel.Error, string.Format(messageFormat, args));
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000AC94 File Offset: 0x00008E94
		public static void LogError(string messageFormat, params object[] args)
		{
			LoggingService.LogUserError(messageFormat, args);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000AC9D File Offset: 0x00008E9D
		public static void LogFatalError(string messageFormat, params object[] args)
		{
			LoggingService.Log(LogLevel.Fatal, string.Format(messageFormat, args));
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000ACAC File Offset: 0x00008EAC
		public static void LogDebug(string message, Exception ex)
		{
			LoggingService.Log(LogLevel.Debug, message + ((ex != null) ? (Environment.NewLine + ex) : string.Empty));
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000ACD0 File Offset: 0x00008ED0
		public static void LogInfo(string message, Exception ex)
		{
			LoggingService.Log(LogLevel.Info, message + ((ex != null) ? (Environment.NewLine + ex) : string.Empty));
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000ACF3 File Offset: 0x00008EF3
		public static void LogWarning(string message, Exception ex)
		{
			LoggingService.Log(LogLevel.Warn, message + ((ex != null) ? (Environment.NewLine + ex) : string.Empty));
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000AD16 File Offset: 0x00008F16
		public static void LogError(string message, Exception ex)
		{
			LoggingService.LogUserError(message, ex);
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000AD1F File Offset: 0x00008F1F
		[Obsolete("Use LogError")]
		public static void LogUserError(string message, Exception ex)
		{
			LoggingService.Log(LogLevel.Error, message + ((ex != null) ? (Environment.NewLine + ex) : string.Empty));
		}

		/// <summary>
		/// Reports that an unexpected error has occurred, but the IDE will continue executing.
		/// Error information is sent to the crash reporting service
		/// </summary>
		/// <param name="ex">Exception</param>
		// Token: 0x060002BC RID: 700 RVA: 0x0000AD42 File Offset: 0x00008F42
		public static void LogInternalError(Exception ex)
		{
			if (ex != null)
			{
				LoggingService.Log(LogLevel.Error, Environment.NewLine + ex);
			}
			LoggingService.ReportUnhandledException(ex, false, true, "internal");
		}

		/// <summary>
		/// Reports that an unexpected error has occurred, but the IDE will continue executing.
		/// Error information is sent to the crash reporting service
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="ex">Exception</param>
		// Token: 0x060002BD RID: 701 RVA: 0x0000AD65 File Offset: 0x00008F65
		public static void LogInternalError(string message, Exception ex)
		{
			LoggingService.Log(LogLevel.Error, message + ((ex != null) ? (Environment.NewLine + ex) : string.Empty));
			LoggingService.ReportUnhandledException(ex, false, true, "internal");
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000AD95 File Offset: 0x00008F95
		[Obsolete("Use LogInternalError")]
		public static void LogCriticalError(string message, Exception ex)
		{
			LoggingService.LogInternalError(message, ex);
		}

		/// <summary>
		/// Reports that a fatal error has occurred, and that the IDE will shut down.
		/// Error information is sent to the crash reporting service
		/// </summary>
		/// <param name="message">Error message</param>
		/// <param name="ex">Exception</param>
		// Token: 0x060002BF RID: 703 RVA: 0x0000AD9E File Offset: 0x00008F9E
		public static void LogFatalError(string message, Exception ex)
		{
			LoggingService.Log(LogLevel.Error, message + ((ex != null) ? (Environment.NewLine + ex) : string.Empty));
			LoggingService.ReportUnhandledException(ex, true, false, "fatal");
		}

		// Token: 0x040000F9 RID: 249
		private const string ServiceVersion = "1";

		// Token: 0x040000FA RID: 250
		private const string ReportCrashesKey = "MonoDevelop.LogAgent.ReportCrashes";

		// Token: 0x040000FB RID: 251
		private const string ReportUsageKey = "MonoDevelop.LogAgent.ReportUsage";

		// Token: 0x040000FC RID: 252
		private static List<ILogger> loggers = new List<ILogger>();

		// Token: 0x040000FD RID: 253
		private static RemoteLogger remoteLogger;

		// Token: 0x040000FE RID: 254
		private static DateTime timestamp;

		// Token: 0x040000FF RID: 255
		private static int logFileSuffix;

		// Token: 0x04000100 RID: 256
		private static TextWriter defaultError;

		// Token: 0x04000101 RID: 257
		private static TextWriter defaultOut;

		// Token: 0x04000102 RID: 258
		private static bool reporting;

		// Token: 0x04000103 RID: 259
		public static Func<bool?, Exception, bool, bool?> UnhandledErrorOccured;

		// Token: 0x04000104 RID: 260
		private static List<CrashReporter> customCrashReporters = new List<CrashReporter>();

		// Token: 0x04000105 RID: 261
		private static LogTextWriter stderr;

		// Token: 0x04000106 RID: 262
		private static LogTextWriter stdout;

		// Token: 0x04000107 RID: 263
		private static TextWriter writer;
	}
}
