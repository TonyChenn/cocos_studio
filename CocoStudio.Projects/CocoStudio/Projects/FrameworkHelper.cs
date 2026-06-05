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
	// Token: 0x0200004F RID: 79
	public class FrameworkHelper
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000863C File Offset: 0x0000683C
		private static IEnumerable<string> ServerVersionFilePaths
		{
			get
			{
				foreach (string text in Option.UpdateServerURLs)
				{
					yield return text + "FrameworkVersionList.xml";
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000864D File Offset: 0x0000684D
		private static string LocalVersionFilePath
		{
			get
			{
				return Path.Combine(Option.FrameworkDir, "FrameworkVersionList.xml");
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00008660 File Offset: 0x00006860
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

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000872C File Offset: 0x0000692C
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

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000877C File Offset: 0x0000697C
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

		// Token: 0x0600022A RID: 554 RVA: 0x000087FC File Offset: 0x000069FC
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

		// Token: 0x0600022B RID: 555 RVA: 0x000088AC File Offset: 0x00006AAC
		private static XElement GetXmlInfoFromServer()
		{
			foreach (string text in FrameworkHelper.ServerVersionFilePaths)
			{
				try
				{
					Uri requestUri = new Uri(text);
					WebRequest webRequest = WebRequest.Create(requestUri);
					webRequest.Credentials = CredentialCache.DefaultCredentials;
					webRequest.Timeout = 2000;
					HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
					Stream responseStream = httpWebResponse.GetResponseStream();
					XElement xelement = XElement.Load(responseStream);
					responseStream.Close();
					if (xelement != null)
					{
						return xelement;
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("从服务器上获取XML信息时出错", exception);
				}
			}
			return null;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x00008934 File Offset: 0x00006B34
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

		// Token: 0x0600022D RID: 557 RVA: 0x00008988 File Offset: 0x00006B88
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

		// Token: 0x0600022E RID: 558 RVA: 0x00008AA0 File Offset: 0x00006CA0
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

		// Token: 0x0600022F RID: 559 RVA: 0x00008AFC File Offset: 0x00006CFC
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

		// Token: 0x06000230 RID: 560 RVA: 0x00008B54 File Offset: 0x00006D54
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

		// Token: 0x06000231 RID: 561 RVA: 0x00008BB4 File Offset: 0x00006DB4
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

		// Token: 0x04000087 RID: 135
		private const string node_CocosStudio = "CocosStudio";

		// Token: 0x04000088 RID: 136
		private const string node_Framework = "Framework";

		// Token: 0x04000089 RID: 137
		private const string attribute_Version = "Version";

		// Token: 0x0400008A RID: 138
		private const string versionFileName = "FrameworkVersionList.xml";

		// Token: 0x0400008B RID: 139
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

		// Token: 0x0400008C RID: 140
		public static IReadOnlyList<string> disableSupplymentVersions = new List<string>
		{
			"cocos2d-x-3.4",
			"cocos2d-x-3.4rc1",
			"cocos2d-x-3.5beta0",
			"cocos2d-x-3.5rc0",
			"cocos2d-x-3.5"
		};

		// Token: 0x0400008D RID: 141
		private static bool hasInitialized = false;

		// Token: 0x0400008E RID: 142
		private static string frameworkBaseDir = string.Empty;

		// Token: 0x0400008F RID: 143
		private static List<string> supportVersion;
	}
}
