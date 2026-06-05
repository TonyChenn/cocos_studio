using System;

namespace Modules.Communal.MultiLanguage
{
	// Token: 0x02000003 RID: 3
	public static class LanguageAdapter
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public static string GetCurrentName(EnumNameFormat format, bool isUsedInServer = false)
		{
			LanguageType language = LanguageOption.CurrentLanguage;
			if (isUsedInServer && !LanguageAdapter.CheckIsServerSurported(language))
			{
				language = LanguageType.English;
			}
			return LanguageAdapter.GetName(language, format);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		private static bool CheckIsServerSurported(LanguageType language)
		{
			return true;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
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

		// Token: 0x06000004 RID: 4 RVA: 0x00002160 File Offset: 0x00000360
		public static string GetLocalizedUrl(string urlFormat)
		{
			string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
			return string.Format(urlFormat, arg);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002188 File Offset: 0x00000388
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
