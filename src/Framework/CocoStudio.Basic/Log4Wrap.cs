using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using log4net;
using log4net.Config;

namespace CocoStudio.Basic
{
	// Token: 0x02000006 RID: 6
	internal class Log4Wrap
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000020A0 File Offset: 0x000002A0
		private static void Init(string configFolder)
		{
			try
			{
				string path = Environment.GetCommandLineArgs()[0];
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
				string text = Path.Combine(configFolder, "log4net_config");
				string text2 = Path.Combine(configFolder, "logs");
				Log4Wrap.logfilename = fileNameWithoutExtension + "_" + DateTime.Now.ToString("o") + ".log";
				Log4Wrap.logfilename = Log4Wrap.logfilename.Replace(":", ".");
				Log4Wrap.logfilename = Path.Combine(text2, Log4Wrap.logfilename);
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				Log4Wrap.configPath = Path.Combine(text, fileNameWithoutExtension + ".xml");
				if (!File.Exists(Log4Wrap.configPath))
				{
					Log4Wrap.CreateDefaultConfigFile(Log4Wrap.configPath);
				}
				else
				{
					XElement xelement = XElement.Load(Log4Wrap.configPath);
					xelement.Descendants("appender").First((XElement n) => n.Attribute("name").Value == "RollingFileAppender").Element("file").Attribute("value").Value = Log4Wrap.logfilename;
					xelement.Save(Log4Wrap.configPath);
				}
				FileInfo configFile = new FileInfo(Log4Wrap.configPath);
				XmlConfigurator.ConfigureAndWatch(configFile);
				Log4Wrap.logger = LogManager.GetLogger("CKLog");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00000448
		public static ILog Logger
		{
			get
			{
				return Log4Wrap.logger;
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0000225F File Offset: 0x0000045F
		internal static void SetLocation(string configFolder)
		{
			Log4Wrap.Init(configFolder);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000D RID: 13 RVA: 0x0000226C File Offset: 0x0000046C
		public static string ConfigPath
		{
			get
			{
				return Log4Wrap.configPath;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002284 File Offset: 0x00000484
		private static void CreateDefaultConfigFile(string filename)
		{
			string value = string.Empty;
			value = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n<configuration>\n    <configSections>\n        <section name=\"log4net\" type=\"log4net.Config.Log4NetConfigurationSectionHandler,log4net-net-1.2\" />\n    </configSections>\n\n    <log4net>\n        <logger name=\"CKLog\">\n            <level value=\"INFO\" />\n            <appender-ref ref=\"RollingFileAppender\" />\n            <appender-ref ref=\"ConsoleAppender\" />\n        </logger>\n\n        <appender name=\"ConsoleAppender\"  type=\"log4net.Appender.ConsoleAppender\" >\n            <layout type=\"log4net.Layout.PatternLayout\">\n                <param name=\"ConversionPattern\"  value=\"%date [%-5level] [Thrd:%thread] %l - %message%newline\"/>\n            </layout>\n        </appender>\n\n        <appender name=\"RollingFileAppender\" type=\"log4net.Appender.RollingFileAppender\">\n            <file value=\"" + Log4Wrap.logfilename + "\" />\n            <appendToFile value=\"true\" />\n            <rollingStyle value=\"Size\" />\n            <maxSizeRollBackups value=\"10\" />\n            <maximumFileSize value=\"500KB\" />\n            <staticLogFileName value=\"true\" />\n            <layout type=\"log4net.Layout.PatternLayout\">\n                <conversionPattern value=\"%date [%-5level] [Thrd:%thread] %l - %message%newline\" />\n            </layout>\n        </appender>\n    </log4net>\n</configuration>";
			try
			{
				StreamWriter streamWriter = File.CreateText(filename);
				streamWriter.Write(value);
				streamWriter.Close();
				streamWriter.Dispose();
			}
			catch (Exception ex)
			{
				string message = ex.Message;
			}
		}

		// Token: 0x04000010 RID: 16
		private const string LOG_FOLDER = "logs";

		// Token: 0x04000011 RID: 17
		private const string LOG_CONFIG_FOLDER = "log4net_config";

		// Token: 0x04000012 RID: 18
		private const string LOGGER_NAME = "CKLog";

		// Token: 0x04000013 RID: 19
		private const string MaxSizeRollBackups = "10";

		// Token: 0x04000014 RID: 20
		private const string MaxFileSize = "500KB";

		// Token: 0x04000015 RID: 21
		private static ILog logger = null;

		// Token: 0x04000016 RID: 22
		private static string configPath = string.Empty;

		// Token: 0x04000017 RID: 23
		private static string logfilename = string.Empty;
	}
}
