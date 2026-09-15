using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Linq;
using CocoStudio.Basic;
using CocoStudio.UserStatistics;
using Microsoft.Win32;
using MonoDevelop.Core;

namespace CocoStudio.Projects
{
	public class FrameworkHelper
	{
		private static string ServerVersionFilePath
		{
			get
			{
				return Option.UpdateServerURL + "FrameworkVersionList.xml";
			}
		}

		private static string LocalVersionFilePath
		{
			get
			{
				return Path.Combine(Option.FrameworkDir, "FrameworkVersionList.xml");
			}
		}

		public static string FrameworkBaseDirectory
		{
			get
			{
				if (string.IsNullOrEmpty(FrameworkHelper.frameworkBaseDir))
				{
					if (Platform.IsWindows)
					{
						try
						{
							object value = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CocosFramework", "InstallDir", null);
							if (value == null || string.IsNullOrEmpty(value.ToString()))
							{
								LogConfig.Logger.Info("未能从注册表中获取到Cocos Framework的安装路径", true);
								FrameworkHelper.frameworkBaseDir = string.Empty;
							}
							else
							{
								FrameworkHelper.frameworkBaseDir = Path.Combine(value.ToString(), "frameworks");
							}
							goto IL_98;
						}
						catch (Exception exception)
						{
							LogConfig.Logger.Error("获取Cocos Framework路径失败", exception);
							FrameworkHelper.frameworkBaseDir = string.Empty;
							goto IL_98;
						}
					}
					if (Platform.IsMac)
					{
						FrameworkHelper.frameworkBaseDir = "/Applications/Cocos/frameworks/";
					}
				}
				IL_98:
				if (Directory.Exists(FrameworkHelper.frameworkBaseDir))
				{
					return FrameworkHelper.frameworkBaseDir;
				}
				return string.Empty;
			}
		}

		public static string SupportNewestVersion
		{
			get
			{
				if (FrameworkHelper.supportVersion == null || FrameworkHelper.supportVersion.Count == 0)
				{
					return string.Empty;
				}
				string text = FrameworkHelper.supportVersion[FrameworkHelper.supportVersion.Count - 1];
				return text.Replace("cocos2d-x-", "");
			}
		}

		public static IReadOnlyList<string> EnabledVersions
		{
			get
			{
				List<string> list = new List<string>();
				if (string.IsNullOrEmpty(FrameworkHelper.FrameworkBaseDirectory))
				{
					return list;
				}
				foreach (string version in FrameworkHelper.supportVersion)
				{
					Cocos2dxInfo framework = Cocos2dxInfo.GetFramework(version);
					if (framework != null)
					{
						list.Add(framework.VersionText);
					}
				}
				return list;
			}
		}

		public static void Initialize()
		{
			if (FrameworkHelper.hasInitialized)
			{
				return;
			}
			FrameworkHelper.supportVersion = new List<string>();
			XElement xmlInfoFromLocalFile = FrameworkHelper.GetXmlInfoFromLocalFile();
			IReadOnlyList<string> readOnlyList = FrameworkHelper.LoadVersionFromXml(xmlInfoFromLocalFile);
			if (readOnlyList.Count == 0)
			{
				readOnlyList = FrameworkHelper.defaultVersions;
			}
			foreach (string item in readOnlyList)
			{
				FrameworkHelper.supportVersion.Add(item);
			}
			Task task = new Task(delegate()
			{
				FrameworkHelper.InitUsingServer();
			});
			task.Start();
			FrameworkHelper.hasInitialized = true;
		}

		private static XElement GetXmlInfoFromServer()
		{
			try
			{
				Uri requestUri = new Uri(FrameworkHelper.ServerVersionFilePath);
				WebRequest webRequest = WebRequest.Create(requestUri);
				webRequest.Credentials = CredentialCache.DefaultCredentials;
				webRequest.Timeout = 2000;
				HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				XElement xelement = XElement.Load(responseStream);
				responseStream.Close();
				return xelement;
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("从服务器上获取XML信息时出错", exception);
			}
			return null;
		}

		private static XElement GetXmlInfoFromLocalFile()
		{
			XElement result;
			try
			{
				if (!File.Exists(FrameworkHelper.LocalVersionFilePath))
				{
					result = null;
				}
				else
				{
					XElement xelement = XElement.Load(FrameworkHelper.LocalVersionFilePath);
					result = xelement;
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("从本地配置文件获取XML信息时出错", exception);
				result = null;
			}
			return result;
		}

		private static List<string> LoadVersionFromXml(XElement xml)
		{
			List<string> list = new List<string>();
			if (xml == null)
			{
				return list;
			}
			try
			{
				IEnumerable<XElement> enumerable = xml.Descendants("CocosStudio");
				foreach (XElement xelement in enumerable)
				{
					if ("2.3.3.0".Equals(xelement.Attribute("Version").Value))
					{
						IEnumerable<XElement> enumerable2 = xelement.Descendants("Framework");
						foreach (XElement xelement2 in enumerable2)
						{
							list.Add(xelement2.Attribute("Version").Value);
						}
					}
				}
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("Failed to load Framework version list from XML.", exception);
				list = new List<string>();
			}
			return list;
		}

		private static void InitUsingServer()
		{
			XElement xmlInfoFromServer = FrameworkHelper.GetXmlInfoFromServer();
			List<string> list = FrameworkHelper.LoadVersionFromXml(xmlInfoFromServer);
			if (list == null || list.Count == 0)
			{
				Tracker.Add(ViewRegions.None, "Cocos Framework", "ConnectFailed", "");
				return;
			}
			Tracker.Add(ViewRegions.None, "Cocos Framework", "ConnectSuceess", "");
			FrameworkHelper.supportVersion = list;
			FrameworkHelper.SaveXmlInfo(xmlInfoFromServer);
		}

		private static void SaveXmlInfo(XElement xml)
		{
			try
			{
				string directoryName = Path.GetDirectoryName(FrameworkHelper.LocalVersionFilePath);
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				xml.Save(FrameworkHelper.LocalVersionFilePath);
			}
			catch (Exception exception)
			{
				LogConfig.Logger.Error("保存编辑器支持的Framework版本列表时出错", exception);
			}
		}

		public static bool IsVersionEnabled(string frameworkVersion)
		{
			if (string.IsNullOrEmpty(frameworkVersion))
			{
				return false;
			}
			foreach (string text in FrameworkHelper.EnabledVersions)
			{
				if (text.Equals(frameworkVersion))
				{
					return true;
				}
			}
			return false;
		}

		public static Version TryParseVersion(string versionText)
		{
			Version result;
			try
			{
				versionText = versionText.ToLower().Replace("cocos2d-x-", "").Replace(" ", "");
				versionText = versionText.Substring(0, 3);
				Version version = new Version(versionText);
				result = version;
			}
			catch
			{
				result = null;
			}
			return result;
		}

		private const string node_CocosStudio = "CocosStudio";

		private const string node_Framework = "Framework";

		private const string attribute_Version = "Version";

		private const string versionFileName = "FrameworkVersionList.xml";

		private static IReadOnlyList<string> defaultVersions = new List<string>
		{
			"cocos2d-x-3.4",
			"cocos2d-x-3.4rc1",
			"cocos2d-x-3.5beta0",
			"cocos2d-x-3.5rc0",
			"cocos2d-x-3.5",
			"cocos2d-x-3.6",
			"cocos2d-x-3.7",
			"cocos2d-x-3.7.1",
			"cocos2d-x-3.8",
			"cocos2d-x-3.8.1",
			"cocos2d-x-3.9"
		};

		public static IReadOnlyList<string> disableSupplymentVersions = new List<string>
		{
			"cocos2d-x-3.4",
			"cocos2d-x-3.4rc1",
			"cocos2d-x-3.5beta0",
			"cocos2d-x-3.5rc0",
			"cocos2d-x-3.5"
		};

		private static bool hasInitialized = false;

		private static string frameworkBaseDir = string.Empty;

		private static List<string> supportVersion;
	}
}
