using System;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace MonoDevelop.Core
{
	// Token: 0x0200004C RID: 76
	public static class PropertyService
	{
		// Token: 0x06000268 RID: 616 RVA: 0x00009B29 File Offset: 0x00007D29
		public static PropertyWrapper<T> Wrap<T>(string property, T defaultValue)
		{
			return new PropertyWrapper<T>(property, defaultValue);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00009B32 File Offset: 0x00007D32
		internal static void Initialize()
		{
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600026A RID: 618 RVA: 0x00009B34 File Offset: 0x00007D34
		public static Properties GlobalInstance
		{
			get
			{
				return PropertyService.properties;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600026B RID: 619 RVA: 0x00009B3B File Offset: 0x00007D3B
		public static FilePath EntryAssemblyPath
		{
			get
			{
				if (Assembly.GetEntryAssembly() != null)
				{
					return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
				}
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		/// <summary>
		/// Location of data files that are bundled with MonoDevelop itself.
		/// </summary>
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600026C RID: 620 RVA: 0x00009B70 File Offset: 0x00007D70
		public static FilePath DataPath
		{
			get
			{
				string text = ConfigurationManager.AppSettings["DataDirectory"];
				if (string.IsNullOrEmpty(text))
				{
					text = Path.Combine(PropertyService.EntryAssemblyPath, Path.Combine("..", "data"));
				}
				return text;
			}
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00009BD0 File Offset: 0x00007DD0
		static PropertyService()
		{
			Counters.PropertyServiceInitialization.BeginTiming();
			string version = null;
			UserProfile userProfile = null;
			FilePath filePath = UserProfile.Current.ConfigDir.Combine(new string[]
			{
				PropertyService.FileName
			});
			if (!File.Exists(filePath))
			{
				if (PropertyService.GetMigratableProfile(out userProfile, out version))
				{
					FilePath filePath2 = userProfile.ConfigDir.Combine(new string[]
					{
						PropertyService.FileName
					});
					try
					{
						FilePath parentDirectory = filePath.ParentDirectory;
						if (!Directory.Exists(parentDirectory))
						{
							Directory.CreateDirectory(parentDirectory);
						}
						File.Copy(filePath2, filePath);
						LoggingService.LogInfo("Migrated core properties from {0}", new object[]
						{
							filePath2
						});
						goto IL_FF;
					}
					catch (IOException ex)
					{
						string message = string.Format("Failed to migrate core properties from {0}", filePath2);
						LoggingService.LogError(message, ex);
						goto IL_FF;
					}
				}
				LoggingService.LogInfo("Did not find previous version from which to migrate data");
			}
			IL_FF:
			if (!PropertyService.LoadProperties(filePath))
			{
				PropertyService.properties = new Properties();
				PropertyService.properties.Set("MonoDevelop.Core.FirstRun", true);
			}
			if (userProfile != null)
			{
				UserDataMigrationService.SetMigrationSource(userProfile, version);
			}
			PropertyService.properties.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
			{
				if (PropertyService.PropertyChanged != null)
				{
					PropertyService.PropertyChanged(sender, args);
				}
			};
			Counters.PropertyServiceInitialization.EndTiming();
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00009D54 File Offset: 0x00007F54
		internal static bool GetMigratableProfile(out UserProfile profile, out string version)
		{
			profile = null;
			version = null;
			int num = UserProfile.ProfileVersions.Length - 2;
			for (int i = num; i >= 1; i--)
			{
				string text = UserProfile.ProfileVersions[i];
				UserProfile profile2 = UserProfile.GetProfile(text);
				if (File.Exists(profile2.ConfigDir.Combine(new string[]
				{
					PropertyService.FileName
				})))
				{
					profile = profile2;
					version = text;
					return true;
				}
			}
			if (BrandingService.ProfileDirectoryName == "MonoDevelop")
			{
				UserProfile userProfile = UserProfile.ForMD24();
				if (File.Exists(userProfile.ConfigDir.Combine(new string[]
				{
					PropertyService.FileName
				})))
				{
					profile = userProfile;
					version = "2.4";
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00009E18 File Offset: 0x00008018
		private static bool LoadProperties(string fileName)
		{
			PropertyService.properties = null;
			if (File.Exists(fileName))
			{
				try
				{
					PropertyService.properties = Properties.Load(fileName);
				}
				catch (Exception ex)
				{
					LoggingService.LogError("Error loading properties from file '{0}':\n{1}", new object[]
					{
						fileName,
						ex
					});
				}
			}
			string text = fileName + ".previous";
			if (PropertyService.properties == null && File.Exists(text))
			{
				try
				{
					PropertyService.properties = Properties.Load(text);
				}
				catch (Exception ex2)
				{
					LoggingService.LogError("Error loading properties from backup file '{0}':\n{1}", new object[]
					{
						text,
						ex2
					});
				}
			}
			return PropertyService.properties != null;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00009ED0 File Offset: 0x000080D0
		public static void SaveProperties()
		{
			FilePath filePath = UserProfile.Current.ConfigDir.Combine(new string[]
			{
				PropertyService.FileName
			});
			Directory.CreateDirectory(filePath.ParentDirectory);
			PropertyService.properties.Save(filePath);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00009F22 File Offset: 0x00008122
		public static bool HasValue(string property)
		{
			return PropertyService.properties.HasValue(property);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00009F2F File Offset: 0x0000812F
		public static T Get<T>(string property, T defaultValue)
		{
			return PropertyService.properties.Get<T>(property, defaultValue);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00009F3D File Offset: 0x0000813D
		public static T Get<T>(string property)
		{
			return PropertyService.properties.Get<T>(property);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00009F4A File Offset: 0x0000814A
		public static void Set(string key, object val)
		{
			PropertyService.properties.Set(key, val);
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00009F58 File Offset: 0x00008158
		public static void AddPropertyHandler(string propertyName, EventHandler<PropertyChangedEventArgs> handler)
		{
			PropertyService.properties.AddPropertyHandler(propertyName, handler);
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00009F66 File Offset: 0x00008166
		public static void RemovePropertyHandler(string propertyName, EventHandler<PropertyChangedEventArgs> handler)
		{
			PropertyService.properties.RemovePropertyHandler(propertyName, handler);
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x06000277 RID: 631 RVA: 0x00009F74 File Offset: 0x00008174
		// (remove) Token: 0x06000278 RID: 632 RVA: 0x00009FA8 File Offset: 0x000081A8
		public static event EventHandler<PropertyChangedEventArgs> PropertyChanged;

		// Token: 0x040000E1 RID: 225
		private static readonly string FileName = "MonoDevelopProperties.xml";

		// Token: 0x040000E2 RID: 226
		private static Properties properties;
	}
}
