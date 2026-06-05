using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using CocoStudio.Basic;

namespace Modules.Communal.MultiLanguage
{
	// Token: 0x02000005 RID: 5
	public static class LanguageOption
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021D8 File Offset: 0x000003D8
		// (set) Token: 0x0600000A RID: 10 RVA: 0x000021EE File Offset: 0x000003EE
		public static LanguageType CurrentLanguage { get; private set; }

		// Token: 0x0600000B RID: 11 RVA: 0x000021F6 File Offset: 0x000003F6
		static LanguageOption()
		{
			LanguageOption.defaultLanguage = LanguageOption.GetDefaultLanguage();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002220 File Offset: 0x00000420
		public static void Init()
		{
			LanguageType languageType = LanguageOption.defaultLanguage;
			if (Option.UserConfig.CustomConfigs.ContainsKey("CCS_LanguageConfig"))
			{
				LanguageConfig languageConfig = Option.UserConfig.CustomConfigs["CCS_LanguageConfig"] as LanguageConfig;
				languageType = languageConfig.LanguageType;
			}
			else
			{
				LanguageType? languageFromOldConfigFile = LanguageOption.GetLanguageFromOldConfigFile();
				if (languageFromOldConfigFile != null)
				{
					languageType = languageFromOldConfigFile.Value;
				}
				LanguageConfig languageConfig = new LanguageConfig();
				languageConfig.LanguageType = languageType;
				Option.UserConfig.CustomConfigs["CCS_LanguageConfig"] = languageConfig;
				Option.UserConfig.Save();
			}
			LanguageOption.Init(languageType);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022C8 File Offset: 0x000004C8
		public static void Init(LanguageType languageType)
		{
			if (!LanguageOption.hasInitialized)
			{
				LanguageOption.CurrentLanguage = languageType;
				if (LanguageOption.CurrentLanguage != LanguageType.Chinese)
				{
					string xmlFilePath = LanguageOption.GetXmlFilePath(LanguageOption.CurrentLanguage);
					Dictionary<string, string> dictionary = LanguageOption.CreateLanguageDictonary(xmlFilePath);
					FieldInfo[] fields = LanguageOption.typeLanguageInfo.GetFields();
					foreach (FieldInfo fieldInfo in fields)
					{
						string name;
						dictionary.TryGetValue(fieldInfo.Name, out name);
						if (string.IsNullOrEmpty(name))
						{
							name = fieldInfo.Name;
						}
						fieldInfo.SetValue(LanguageOption.typeLanguageInfo, name);
					}
				}
				LanguageOption.hasInitialized = true;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002380 File Offset: 0x00000580
		public static string GetValueBykey(string key)
		{
			if (!LanguageOption.hasInitialized)
			{
				LanguageOption.Init();
			}
			string result;
			if (string.IsNullOrEmpty(key))
			{
				result = "";
			}
			else
			{
				FieldInfo field = LanguageOption.typeLanguageInfo.GetField(key);
				if (field != null)
				{
					result = field.GetValue(key).ToString();
				}
				else
				{
					result = key;
				}
			}
			return result;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000023E4 File Offset: 0x000005E4
		public static void SetEditorLanguage(LanguageType languageType)
		{
			LanguageConfig languageConfig = null;
			if (Option.UserConfig.CustomConfigs.ContainsKey("CCS_LanguageConfig"))
			{
				languageConfig = (Option.UserConfig.CustomConfigs["CCS_LanguageConfig"] as LanguageConfig);
			}
			if (languageConfig == null)
			{
				languageConfig = new LanguageConfig();
				Option.UserConfig.CustomConfigs["CCS_LanguageConfig"] = languageConfig;
			}
			languageConfig.LanguageType = languageType;
			Option.UserConfig.Save();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002468 File Offset: 0x00000668
		private static LanguageType GetDefaultLanguage()
		{
			LanguageType result = LanguageType.English;
			CultureInfo currentCulture = CultureInfo.CurrentCulture;
			string text = currentCulture.ToString();
			if (text.StartsWith("zh-"))
			{
				if (text.Equals("zh-CN"))
				{
					result = LanguageType.Chinese;
				}
				else
				{
					result = LanguageType.Traditional;
				}
			}
			else if (text.Equals("en-US"))
			{
				result = LanguageType.English;
			}
			return result;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x000024D4 File Offset: 0x000006D4
		private static LanguageType? GetLanguageFromOldConfigFile()
		{
			string userConfigFileByName = Option.GetUserConfigFileByName("LanguageDefaultConfig.xml");
			LanguageType? result;
			if (!File.Exists(userConfigFileByName))
			{
				result = null;
			}
			else
			{
				try
				{
					XElement xelement = XElement.Load(userConfigFileByName);
					XElement xelement2 = xelement.Element("Item");
					string value = xelement2.Attribute("Default").Value;
					if (string.IsNullOrEmpty(value))
					{
						result = null;
					}
					else if (value.Equals("en-CHS") || value.Equals("zh-CN"))
					{
						result = new LanguageType?(LanguageType.Chinese);
					}
					else
					{
						result = new LanguageType?(LanguageType.English);
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("从旧版本配置文件中读取显示语言类型时出错", exception);
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000025C8 File Offset: 0x000007C8
		private static string GetMarkFromType(LanguageType type)
		{
			string result = "en-US";
			switch (type)
			{
			case LanguageType.English:
				result = "en-US";
				break;
			case LanguageType.Chinese:
				result = "zh-CN";
				break;
			case LanguageType.Traditional:
				result = "zh-TW";
				break;
			}
			return result;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002610 File Offset: 0x00000810
		private static void WriteChineseXml()
		{
			string xmlFilePath = LanguageOption.GetXmlFilePath(LanguageType.Chinese);
			if (File.Exists(xmlFilePath))
			{
				File.Delete(xmlFilePath);
			}
			string directoryName = Path.GetDirectoryName(xmlFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (File.Create(xmlFilePath))
			{
			}
			XElement xelement = new XElement("zh-CN");
			FieldInfo[] fields = LanguageOption.typeLanguageInfo.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				string value = fieldInfo.GetValue(LanguageOption.typeLanguageInfo) as string;
				if (string.IsNullOrEmpty(value))
				{
					xelement.Add(LanguageOption.CreateXmlItem(fieldInfo.Name, string.Empty));
					xelement.Add(new XElement("InfoMissing"));
				}
				else
				{
					xelement.Add(LanguageOption.CreateXmlItem(fieldInfo.Name, value));
				}
			}
			xelement.Save(xmlFilePath);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000273C File Offset: 0x0000093C
		private static void WriteLanguageXml(LanguageType type)
		{
			string xmlFilePath = LanguageOption.GetXmlFilePath(type);
			string directoryName = Path.GetDirectoryName(xmlFilePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			XElement xelement = new XElement(LanguageOption.GetMarkFromType(type));
			FieldInfo[] fields = LanguageOption.typeLanguageInfo.GetFields();
			if (File.Exists(xmlFilePath))
			{
				Dictionary<string, string> dictionary = LanguageOption.CreateLanguageDictonary(xmlFilePath);
				foreach (FieldInfo fieldInfo in fields)
				{
					string value;
					dictionary.TryGetValue(fieldInfo.Name, out value);
					if (string.IsNullOrEmpty(value))
					{
						xelement.Add(LanguageOption.CreateXmlItem(fieldInfo.Name, ""));
						xelement.Add(new XElement("InfoMissing"));
					}
					else
					{
						xelement.Add(LanguageOption.CreateXmlItem(fieldInfo.Name, value));
					}
				}
				xelement.Save(xmlFilePath);
			}
			else
			{
				File.Delete(xmlFilePath);
				using (File.Create(xmlFilePath))
				{
				}
				foreach (FieldInfo fieldInfo in fields)
				{
					xelement.Add(LanguageOption.CreateXmlItem(fieldInfo.Name, ""));
					xelement.Add(new XElement("InfoMissing"));
				}
				xelement.Save(xmlFilePath);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000028DC File Offset: 0x00000ADC
		private static string GetXmlFilePath(LanguageType languageType)
		{
			string path = LanguageOption.GetMarkFromType(languageType) + ".xml";
			string fullName = Directory.GetParent(typeof(LanguageOption).Assembly.Location).FullName;
			return Path.Combine(fullName, "LanguageResource", path);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000292C File Offset: 0x00000B2C
		private static Dictionary<string, string> CreateLanguageDictonary(string xmlFilePath)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			Dictionary<string, string> result;
			if (string.IsNullOrEmpty(xmlFilePath))
			{
				result = dictionary;
			}
			else if (!File.Exists(xmlFilePath))
			{
				result = dictionary;
			}
			else
			{
				try
				{
					using (FileStream fileStream = new FileStream(xmlFilePath, FileMode.Open, FileAccess.Read))
					{
						TextReader textReader = new StreamReader(fileStream);
						string text = textReader.ReadToEnd();
						textReader.Close();
						XElement xelement = XElement.Parse(text);
						IEnumerable<XElement> enumerable = xelement.Descendants("Item");
						foreach (XElement xelement2 in enumerable)
						{
							string value = xelement2.Attribute("Key").Value;
							string value2 = xelement2.Attribute("Value").Value;
							dictionary.Add(value, value2);
						}
						result = dictionary;
					}
				}
				catch (Exception exception)
				{
					LogConfig.Logger.Error("创建语言信息字典失败", exception);
					result = new Dictionary<string, string>();
				}
			}
			return result;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A7C File Offset: 0x00000C7C
		private static XElement CreateXmlItem(string key, string value)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Clear();
			dictionary.Add(key, value);
			XElement xelement = new XElement("Item");
			xelement.SetAttributeValue("Key", key);
			xelement.SetAttributeValue("Value", value);
			return xelement;
		}

		// Token: 0x04000007 RID: 7
		private const string markSimpleChinese = "zh-CN";

		// Token: 0x04000008 RID: 8
		private const string markOldSimpleChinese = "en-CHS";

		// Token: 0x04000009 RID: 9
		private const string markTraditionalChinese = "zh-TW";

		// Token: 0x0400000A RID: 10
		private const string markEnglish = "en-US";

		// Token: 0x0400000B RID: 11
		private const string languageConfigFileName = "LanguageDefaultConfig.xml";

		// Token: 0x0400000C RID: 12
		private const string folderName = "LanguageResource";

		// Token: 0x0400000D RID: 13
		private static bool hasInitialized = false;

		// Token: 0x0400000E RID: 14
		private static Type typeLanguageInfo = typeof(LanguageInfo);

		// Token: 0x0400000F RID: 15
		private static LanguageType defaultLanguage = LanguageType.English;
	}
}
