using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace MonoDevelop.Core
{
	/// <summary>
	/// Access to branding information. Only the ApplicationName is guaranteed to be non-null.
	/// </summary>
	// Token: 0x02000227 RID: 551
	public static class BrandingService
	{
		// Token: 0x060014AC RID: 5292 RVA: 0x00055380 File Offset: 0x00053580
		static BrandingService()
		{
			try
			{
				BrandingService.brandingDir = typeof(BrandingService).Assembly.Location.ParentDirectory.Combine(new string[]
				{
					"branding"
				});
				if (!Directory.Exists(BrandingService.brandingDir))
				{
					BrandingService.brandingDir = null;
				}
				else
				{
					string twoLetterISOLanguageName = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
					BrandingService.localizedBrandingDir = BrandingService.brandingDir.Combine(new string[]
					{
						twoLetterISOLanguageName
					});
					if (!Directory.Exists(BrandingService.localizedBrandingDir))
					{
						BrandingService.localizedBrandingDir = null;
					}
				}
				if (BrandingService.brandingDir != null)
				{
					FilePath filePath = BrandingService.brandingDir.Combine(new string[]
					{
						"Branding.xml"
					});
					if (File.Exists(filePath))
					{
						BrandingService.brandingDocument = XDocument.Load(filePath);
					}
					if (BrandingService.localizedBrandingDir != null)
					{
						FilePath filePath2 = BrandingService.brandingDir.Combine(new string[]
						{
							"Branding.xml"
						});
						if (File.Exists(filePath2))
						{
							BrandingService.localizedBrandingDocument = XDocument.Load(filePath2);
						}
					}
				}
				BrandingService.ApplicationName = BrandingService.GetString(new string[]
				{
					"ApplicationName"
				});
				BrandingService.SuiteName = BrandingService.GetString(new string[]
				{
					"SuiteName"
				});
				BrandingService.ProfileDirectoryName = BrandingService.GetString(new string[]
				{
					"ProfileDirectoryName"
				});
				BrandingService.StatusSteadyIconId = BrandingService.GetString(new string[]
				{
					"StatusAreaSteadyIcon"
				});
			}
			catch (Exception ex)
			{
				LoggingService.LogError("Could not read branding document", ex);
			}
			if (string.IsNullOrEmpty(BrandingService.ApplicationName))
			{
				BrandingService.ApplicationName = "MonoDevelop";
			}
			if (string.IsNullOrEmpty(BrandingService.SuiteName))
			{
				BrandingService.SuiteName = BrandingService.ApplicationName;
			}
			if (string.IsNullOrEmpty(BrandingService.ProfileDirectoryName))
			{
				BrandingService.ProfileDirectoryName = BrandingService.ApplicationName;
			}
			if (string.IsNullOrEmpty(BrandingService.StatusSteadyIconId))
			{
				BrandingService.StatusSteadyIconId = "md-status-steady";
			}
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x000555CC File Offset: 0x000537CC
		public static string GetString(params string[] keyPath)
		{
			XElement element = BrandingService.GetElement(keyPath);
			if (element != null)
			{
				return (string)element;
			}
			return null;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x000555EB File Offset: 0x000537EB
		public static int? GetInt(params string[] keyPath)
		{
			return (int?)BrandingService.GetElement(keyPath);
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x000555F8 File Offset: 0x000537F8
		public static bool? GetBool(params string[] keyPath)
		{
			return (bool?)BrandingService.GetElement(keyPath);
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x00055608 File Offset: 0x00053808
		public static XElement GetElement(params string[] keyPath)
		{
			if (keyPath == null)
			{
				throw new ArgumentNullException();
			}
			if (keyPath.Length == 0)
			{
				throw new ArgumentException();
			}
			if (BrandingService.localizedBrandingDocument != null)
			{
				XElement element = BrandingService.GetElement(BrandingService.localizedBrandingDocument, keyPath);
				if (element != null)
				{
					return element;
				}
			}
			if (BrandingService.brandingDocument != null)
			{
				XElement element2 = BrandingService.GetElement(BrandingService.brandingDocument, keyPath);
				if (element2 != null)
				{
					return element2;
				}
			}
			return null;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x0005565C File Offset: 0x0005385C
		private static XElement GetElement(XDocument doc, string[] keyPath)
		{
			int num = 0;
			XElement xelement = doc.Root;
			do
			{
				xelement = xelement.Element(keyPath[num++]);
			}
			while (num < keyPath.Length && xelement != null);
			return xelement;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00055690 File Offset: 0x00053890
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static FilePath GetFile(string name)
		{
			if (BrandingService.localizedBrandingDir != null)
			{
				FilePath filePath = BrandingService.localizedBrandingDir.Combine(new string[]
				{
					name
				});
				if (File.Exists(filePath))
				{
					return filePath;
				}
			}
			if (BrandingService.brandingDir != null)
			{
				FilePath filePath2 = BrandingService.brandingDir.Combine(new string[]
				{
					name
				});
				if (File.Exists(filePath2))
				{
					return filePath2;
				}
			}
			return null;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00055714 File Offset: 0x00053914
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Stream GetStream(string name, bool lookInCallingAssembly = false)
		{
			FilePath file = BrandingService.GetFile(name);
			if (file != null)
			{
				return File.OpenRead(file);
			}
			if (lookInCallingAssembly)
			{
				return Assembly.GetCallingAssembly().GetManifestResourceStream(name);
			}
			return null;
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x00055752 File Offset: 0x00053952
		public static string BrandApplicationName(string s)
		{
			return s.Replace("MonoDevelop", BrandingService.ApplicationName);
		}

		// Token: 0x0400063E RID: 1598
		private static FilePath brandingDir;

		// Token: 0x0400063F RID: 1599
		private static FilePath localizedBrandingDir;

		// Token: 0x04000640 RID: 1600
		private static XDocument brandingDocument;

		// Token: 0x04000641 RID: 1601
		private static XDocument localizedBrandingDocument;

		// Token: 0x04000642 RID: 1602
		public static readonly string ApplicationName;

		// Token: 0x04000643 RID: 1603
		public static readonly string SuiteName;

		// Token: 0x04000644 RID: 1604
		public static readonly string ProfileDirectoryName;

		// Token: 0x04000645 RID: 1605
		public static readonly string StatusSteadyIconId;
	}
}
