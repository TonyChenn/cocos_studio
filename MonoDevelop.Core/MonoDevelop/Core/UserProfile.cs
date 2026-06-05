using System;

namespace MonoDevelop.Core
{
	// Token: 0x02000225 RID: 549
	public class UserProfile
	{
		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x06001493 RID: 5267 RVA: 0x00054C57 File Offset: 0x00052E57
		// (set) Token: 0x06001494 RID: 5268 RVA: 0x00054C5E File Offset: 0x00052E5E
		public static UserProfile Current { get; private set; } = UserProfile.GetProfile(UserProfile.ProfileVersions[UserProfile.ProfileVersions.Length - 1]);

		/// <summary>Location for cached data that can be regenerated.</summary>
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06001495 RID: 5269 RVA: 0x00054C66 File Offset: 0x00052E66
		// (set) Token: 0x06001496 RID: 5270 RVA: 0x00054C6E File Offset: 0x00052E6E
		public FilePath CacheDir { get; private set; }

		/// <summary>Location for current preferences/settings.</summary>
		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06001497 RID: 5271 RVA: 0x00054C77 File Offset: 0x00052E77
		// (set) Token: 0x06001498 RID: 5272 RVA: 0x00054C7F File Offset: 0x00052E7F
		public FilePath ConfigDir { get; private set; }

		/// <summary>Preferences/settings specific to the local machine.</summary>
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001499 RID: 5273 RVA: 0x00054C88 File Offset: 0x00052E88
		// (set) Token: 0x0600149A RID: 5274 RVA: 0x00054C90 File Offset: 0x00052E90
		public FilePath LocalConfigDir { get; private set; }

		/// <summary>User-visible root location for user-created data files such as templates, snippets and color schemes.</summary>
		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600149B RID: 5275 RVA: 0x00054C99 File Offset: 0x00052E99
		// (set) Token: 0x0600149C RID: 5276 RVA: 0x00054CA1 File Offset: 0x00052EA1
		public FilePath UserDataRoot { get; private set; }

		/// <summary>Location for log files.</summary>
		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600149D RID: 5277 RVA: 0x00054CAA File Offset: 0x00052EAA
		// (set) Token: 0x0600149E RID: 5278 RVA: 0x00054CB2 File Offset: 0x00052EB2
		public FilePath LogDir { get; private set; }

		/// <summary>Location for files installed from external sources.</summary>
		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600149F RID: 5279 RVA: 0x00054CBB File Offset: 0x00052EBB
		// (set) Token: 0x060014A0 RID: 5280 RVA: 0x00054CC3 File Offset: 0x00052EC3
		public FilePath LocalInstallDir { get; private set; }

		/// <summary>Location for temporary files.</summary>
		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x060014A1 RID: 5281 RVA: 0x00054CCC File Offset: 0x00052ECC
		// (set) Token: 0x060014A2 RID: 5282 RVA: 0x00054CD4 File Offset: 0x00052ED4
		public FilePath TempDir { get; private set; }

		/// <summary>Gets a location by its ID.</summary>
		// Token: 0x060014A3 RID: 5283 RVA: 0x00054CE0 File Offset: 0x00052EE0
		internal FilePath GetLocation(UserDataKind kind)
		{
			switch (kind)
			{
			case UserDataKind.Cache:
				return this.CacheDir;
			case UserDataKind.Config:
				return this.ConfigDir;
			case UserDataKind.LocalConfig:
				return this.LocalConfigDir;
			case UserDataKind.UserData:
				return this.UserDataRoot;
			case UserDataKind.Logs:
				return this.LogDir;
			case UserDataKind.LocalInstall:
				return this.LocalInstallDir;
			case UserDataKind.Temp:
				return this.TempDir;
			default:
				throw new ArgumentException("Unknown UserDataLocation:" + kind.ToString());
			}
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00054D60 File Offset: 0x00052F60
		internal static UserProfile GetProfile(string profileVersion)
		{
			FilePath profileLocation = Environment.GetEnvironmentVariable("MONODEVELOP_PROFILE");
			if (!profileLocation.IsNullOrEmpty)
			{
				return UserProfile.ForTest(profileVersion, profileLocation);
			}
			if (Platform.IsWindows)
			{
				return UserProfile.ForWindows(profileVersion);
			}
			if (Platform.IsMac)
			{
				return UserProfile.ForMac(profileVersion);
			}
			return UserProfile.ForUnix(profileVersion);
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00054DB0 File Offset: 0x00052FB0
		private static string GetAppId(string version)
		{
			return BrandingService.ProfileDirectoryName + "-" + version;
		}

		/// <summary>
		/// Creates locations in a specific folder, for testing.
		/// </summary>
		// Token: 0x060014A6 RID: 5286 RVA: 0x00054DC4 File Offset: 0x00052FC4
		internal static UserProfile ForTest(string version, FilePath profileLocation)
		{
			string appId = UserProfile.GetAppId(version);
			return new UserProfile
			{
				CacheDir = profileLocation.Combine(new string[]
				{
					appId,
					"Cache"
				}),
				UserDataRoot = profileLocation.Combine(new string[]
				{
					appId,
					"UserData"
				}),
				ConfigDir = profileLocation.Combine(new string[]
				{
					appId,
					"Config"
				}),
				LocalConfigDir = profileLocation.Combine(new string[]
				{
					appId,
					"LocalConfig"
				}),
				LogDir = profileLocation.Combine(new string[]
				{
					appId,
					"Logs"
				}),
				LocalInstallDir = profileLocation.Combine(new string[]
				{
					appId,
					"LocalInstall"
				}),
				TempDir = profileLocation.Combine(new string[]
				{
					appId,
					"Temp"
				})
			};
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00054EDC File Offset: 0x000530DC
		internal static UserProfile ForWindows(string version)
		{
			string appId = UserProfile.GetAppId(version);
			FilePath filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			FilePath userDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			filePath = filePath.Combine(new string[]
			{
				appId
			});
			userDataRoot = userDataRoot.Combine(new string[]
			{
				appId
			});
			return new UserProfile
			{
				UserDataRoot = userDataRoot,
				ConfigDir = userDataRoot.Combine(new string[]
				{
					"Config"
				}),
				LocalConfigDir = filePath.Combine(new string[]
				{
					"Config"
				}),
				LocalInstallDir = filePath.Combine(new string[]
				{
					"LocalInstall"
				}),
				LogDir = filePath.Combine(new string[]
				{
					"Logs"
				}),
				CacheDir = filePath.Combine(new string[]
				{
					"Cache"
				}),
				TempDir = filePath.Combine(new string[]
				{
					"Temp"
				})
			};
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00055008 File Offset: 0x00053208
		internal static UserProfile ForMac(string version)
		{
			string appId = UserProfile.GetAppId(version);
			FilePath filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).Combine(new string[]
			{
				"Library"
			});
			FilePath userDataRoot = filePath.Combine(new string[]
			{
				appId
			});
			FilePath filePath2 = filePath.Combine(new string[]
			{
				"Preferences",
				appId
			});
			FilePath cacheDir = filePath.Combine(new string[]
			{
				"Caches",
				appId
			});
			FilePath logDir = filePath.Combine(new string[]
			{
				"Logs",
				appId
			});
			FilePath filePath3 = filePath.Combine(new string[]
			{
				"Application Support",
				appId
			});
			return new UserProfile
			{
				CacheDir = cacheDir,
				UserDataRoot = userDataRoot,
				ConfigDir = filePath2,
				LocalConfigDir = filePath2,
				LogDir = logDir,
				LocalInstallDir = filePath3.Combine(new string[]
				{
					"LocalInstall"
				}),
				TempDir = cacheDir.Combine(new string[]
				{
					"Temp"
				})
			};
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00055158 File Offset: 0x00053358
		internal static UserProfile ForUnix(string version)
		{
			FilePath filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			FilePath filePath2 = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
			if (filePath2.IsNullOrEmpty)
			{
				filePath2 = filePath.Combine(new string[]
				{
					".local",
					"share"
				});
			}
			FilePath filePath3 = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
			if (filePath3.IsNullOrEmpty)
			{
				filePath3 = filePath.Combine(new string[]
				{
					".config"
				});
			}
			FilePath filePath4 = Environment.GetEnvironmentVariable("XDG_CACHE_HOME");
			if (filePath4.IsNullOrEmpty)
			{
				filePath4 = filePath.Combine(new string[]
				{
					".cache"
				});
			}
			string appId = UserProfile.GetAppId(version);
			FilePath userDataRoot = filePath2.Combine(new string[]
			{
				appId
			});
			FilePath filePath5 = filePath3.Combine(new string[]
			{
				appId
			});
			FilePath cacheDir = filePath4.Combine(new string[]
			{
				appId
			});
			return new UserProfile
			{
				UserDataRoot = userDataRoot,
				LocalInstallDir = userDataRoot.Combine(new string[]
				{
					"LocalInstall"
				}),
				ConfigDir = filePath5,
				LocalConfigDir = filePath5,
				CacheDir = cacheDir,
				TempDir = cacheDir.Combine(new string[]
				{
					"Temp"
				}),
				LogDir = cacheDir.Combine(new string[]
				{
					"Logs"
				})
			};
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x000552FC File Offset: 0x000534FC
		internal static UserProfile ForMD24()
		{
			FilePath filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Combine(new string[]
			{
				"MonoDevelop"
			});
			return new UserProfile
			{
				UserDataRoot = filePath,
				ConfigDir = filePath,
				LocalConfigDir = filePath,
				LocalInstallDir = filePath.Combine(new string[]
				{
					"addins"
				}),
				LogDir = filePath,
				CacheDir = filePath
			};
		}

		// Token: 0x0400062C RID: 1580
		private const string PROFILE_ENV_VAR = "MONODEVELOP_PROFILE";

		// Token: 0x0400062D RID: 1581
		internal static string[] ProfileVersions = new string[]
		{
			"2.4",
			"2.6",
			"2.7",
			"2.8",
			"3.0",
			"4.0",
			"5.0"
		};
	}
}
