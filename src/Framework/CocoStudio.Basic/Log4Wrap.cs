using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using log4net;
using log4net.Config;

namespace CocoStudio.Basic
{
	internal class Log4Wrap
	{
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

		public static ILog Logger
		{
			get
			{
				return Log4Wrap.logger;
			}
		}

		internal static void SetLocation(string configFolder)
		{
			Log4Wrap.Init(configFolder);
		}

		public static string ConfigPath
		{
			get
			{
				return Log4Wrap.configPath;
			}
		}

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

		private const string LOG_FOLDER = "logs";

		private const string LOG_CONFIG_FOLDER = "log4net_config";

		private const string LOGGER_NAME = "CKLog";

		private const string MaxSizeRollBackups = "10";

		private const string MaxFileSize = "500KB";

		private static ILog logger = null;

		private static string configPath = string.Empty;

		private static string logfilename = string.Empty;
	}
}
