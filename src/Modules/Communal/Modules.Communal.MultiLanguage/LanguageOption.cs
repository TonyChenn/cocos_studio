using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using CocoStudio.Basic;

namespace Modules.Communal.MultiLanguage
{
	public static class LanguageOption
	{
		public static LanguageType CurrentLanguage { get; private set; }

		static LanguageOption()
		{
			LanguageOption.defaultLanguage = LanguageOption.GetDefaultLanguage();
		}

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

		private static string GetXmlFilePath(LanguageType languageType)
		{
			string path = LanguageOption.GetMarkFromType(languageType) + ".xml";
			string fullName = Directory.GetParent(typeof(LanguageOption).Assembly.Location).FullName;
			return Path.Combine(fullName, "LanguageResource", path);
		}

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

		private const string markSimpleChinese = "zh-CN";

		private const string markOldSimpleChinese = "en-CHS";

		private const string markTraditionalChinese = "zh-TW";

		private const string markEnglish = "en-US";

		private const string languageConfigFileName = "LanguageDefaultConfig.xml";

		private const string folderName = "LanguageResource";

		private static bool hasInitialized = false;

		private static Type typeLanguageInfo = typeof(LanguageInfo);

		private static LanguageType defaultLanguage = LanguageType.English;
	}
}
