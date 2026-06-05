using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Mono.Unix;

namespace MonoDevelop.Core
{
	// Token: 0x02000093 RID: 147
	public static class GettextCatalog
	{
		// Token: 0x060004CF RID: 1231
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int SetThreadUILanguage(int LangId);

		// Token: 0x060004D0 RID: 1232 RVA: 0x00010B40 File Offset: 0x0000ED40
		static GettextCatalog()
		{
			string text = Environment.GetEnvironmentVariable("MONODEVELOP_LOCALE_PATH");
			string text2 = PropertyService.Get<string>("MonoDevelop.Ide.UserInterfaceLanguage", "");
			if (!string.IsNullOrEmpty(text2))
			{
				if (Platform.IsWindows)
				{
					text2 = text2.Replace("_", "-");
					CultureInfo cultureInfo = CultureInfo.GetCultureInfo(text2);
					if (cultureInfo.IsNeutralCulture)
					{
						foreach (CultureInfo cultureInfo2 in CultureInfo.GetCultures(CultureTypes.SpecificCultures | CultureTypes.InstalledWin32Cultures))
						{
							if (cultureInfo2.Parent != null && cultureInfo2.Parent.Name == cultureInfo.Name)
							{
								cultureInfo = cultureInfo2;
								break;
							}
						}
					}
					if (!cultureInfo.IsNeutralCulture)
					{
						GettextCatalog.SetThreadUILanguage(cultureInfo.LCID);
						GettextCatalog.mainThread.CurrentUICulture = cultureInfo;
					}
				}
				else
				{
					Environment.SetEnvironmentVariable("LANGUAGE", text2);
				}
			}
			if (string.IsNullOrEmpty(text) || !Directory.Exists(text))
			{
				string text3 = Assembly.GetExecutingAssembly().Location;
				text3 = Path.GetDirectoryName(text3);
				if (Platform.IsWindows)
				{
					text = Path.Combine(text3, "locale");
				}
				else
				{
					string text4 = Path.Combine(Path.Combine(Path.Combine(text3, ".."), ".."), "..");
					if (Platform.IsMac)
					{
						text4 = Path.Combine(text4, "..", "MacOS");
					}
					text4 = Path.GetFullPath(text4);
					text = Path.Combine(Path.Combine(text4, "share"), "locale");
				}
			}
			try
			{
				Catalog.Init("monodevelop", text);
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004D1 RID: 1233 RVA: 0x00010CDC File Offset: 0x0000EEDC
		public static CultureInfo UICulture
		{
			get
			{
				return GettextCatalog.mainThread.CurrentUICulture;
			}
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x00010CE8 File Offset: 0x0000EEE8
		public static string GetString(string phrase)
		{
			return Catalog.GetString(phrase);
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x00010CF0 File Offset: 0x0000EEF0
		public static string GetString(string phrase, object arg0)
		{
			return string.Format(Catalog.GetString(phrase), arg0);
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x00010CFE File Offset: 0x0000EEFE
		public static string GetString(string phrase, object arg0, object arg1)
		{
			return string.Format(Catalog.GetString(phrase), arg0, arg1);
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x00010D0D File Offset: 0x0000EF0D
		public static string GetString(string phrase, object arg0, object arg1, object arg2)
		{
			return string.Format(Catalog.GetString(phrase), arg0, arg1, arg2);
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x00010D1D File Offset: 0x0000EF1D
		public static string GetString(string phrase, params object[] args)
		{
			return string.Format(Catalog.GetString(phrase), args);
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x00010D2B File Offset: 0x0000EF2B
		public static string GetPluralString(string singular, string plural, int number)
		{
			return Catalog.GetPluralString(singular, plural, number);
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x00010D35 File Offset: 0x0000EF35
		public static string GetPluralString(string singular, string plural, int number, object arg0)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), arg0);
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x00010D45 File Offset: 0x0000EF45
		public static string GetPluralString(string singular, string plural, int number, object arg0, object arg1)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), arg0, arg1);
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x00010D57 File Offset: 0x0000EF57
		public static string GetPluralString(string singular, string plural, int number, object arg0, object arg1, object arg2)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), arg0, arg1, arg2);
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x00010D6B File Offset: 0x0000EF6B
		public static string GetPluralString(string singular, string plural, int number, params object[] args)
		{
			return string.Format(Catalog.GetPluralString(singular, plural, number), args);
		}

		// Token: 0x04000191 RID: 401
		private static Thread mainThread = Thread.CurrentThread;
	}
}
