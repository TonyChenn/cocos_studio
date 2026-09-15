using System;

namespace Modules.Communal.MultiLanguage
{
	public static class LanguageAdapter
	{
		public static string GetCurrentName(EnumNameFormat format, bool isUsedInServer = false)
		{
			LanguageType language = LanguageOption.CurrentLanguage;
			if (isUsedInServer && !LanguageAdapter.CheckIsServerSurported(language))
			{
				language = LanguageType.English;
			}
			return LanguageAdapter.GetName(language, format);
		}

		private static bool CheckIsServerSurported(LanguageType language)
		{
			return true;
		}

		private static string GetName(LanguageType language, EnumNameFormat format)
		{
			string result;
			switch (format)
			{
			case EnumNameFormat.Full:
				if (language == LanguageType.Custom)
				{
					result = LanguageType.English.ToString();
				}
				else
				{
					result = language.ToString();
				}
				break;
			case EnumNameFormat.Code:
				switch (language)
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
				default:
					result = "en-US";
					break;
				}
				break;
			case EnumNameFormat.Short:
				switch (language)
				{
				case LanguageType.English:
					result = "en";
					break;
				case LanguageType.Chinese:
					result = "cn";
					break;
				case LanguageType.Traditional:
					result = "tw";
					break;
				default:
					result = "en";
					break;
				}
				break;
			default:
				result = string.Empty;
				break;
			}
			return result;
		}

		public static string GetLocalizedUrl(string urlFormat)
		{
			string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
			return string.Format(urlFormat, arg);
		}

		public static int GetDefaultFontSize()
		{
			int result;
			if (LanguageOption.CurrentLanguage == LanguageType.English)
			{
				result = 11;
			}
			else
			{
				result = 12;
			}
			return result;
		}
	}
}
