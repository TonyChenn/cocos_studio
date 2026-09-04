using System;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Core
{
	// Token: 0x02000035 RID: 53
	[Extension(typeof(IUserConfig))]
	public class RemindService : IUserConfig
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x00009674 File Offset: 0x00007874
		// (set) Token: 0x060001F8 RID: 504 RVA: 0x0000968B File Offset: 0x0000788B
		[ItemProperty("IsShowSimulatorHint/Value")]
		public bool IsShowSimulatorHint { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x00009694 File Offset: 0x00007894
		// (set) Token: 0x060001FA RID: 506 RVA: 0x000096AB File Offset: 0x000078AB
		[ItemProperty("IsShowSkeletonTrackPoint/Value")]
		public bool IsShowSkeletonTrackPoint { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001FB RID: 507 RVA: 0x000096B4 File Offset: 0x000078B4
		public static RemindService Instance
		{
			get
			{
				RemindService instance;
				if (RemindService._Instance != null)
				{
					instance = RemindService._Instance;
				}
				else
				{
					RemindService._Instance = null;
					if (Option.UserConfig.CustomConfigs.ContainsKey("RemindConfig"))
					{
						RemindService._Instance = (Option.UserConfig.CustomConfigs["RemindConfig"] as RemindService);
					}
					if (RemindService._Instance == null)
					{
						RemindService._Instance = new RemindService();
						Option.UserConfig.CustomConfigs["RemindConfig"] = RemindService._Instance;
						Option.UserConfig.Save();
					}
					instance = RemindService._Instance;
				}
				return instance;
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00009761 File Offset: 0x00007961
		private RemindService()
		{
			this.IsShowSimulatorHint = true;
			this.IsShowSkeletonTrackPoint = true;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000977C File Offset: 0x0000797C
		public void Save()
		{
			Option.UserConfig.Save();
		}

		// Token: 0x04000110 RID: 272
		public const string conifgKey = "RemindConfig";

		// Token: 0x04000111 RID: 273
		private static RemindService _Instance;
	}
}
