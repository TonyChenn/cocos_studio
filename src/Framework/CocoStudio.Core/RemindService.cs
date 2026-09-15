using System;
using CocoStudio.Basic;
using Mono.Addins;
using MonoDevelop.Core.Serialization;

namespace CocoStudio.Core
{
	[Extension(typeof(IUserConfig))]
	public class RemindService : IUserConfig
	{
		[ItemProperty("IsShowSimulatorHint/Value")]
		public bool IsShowSimulatorHint { get; set; }

		[ItemProperty("IsShowSkeletonTrackPoint/Value")]
		public bool IsShowSkeletonTrackPoint { get; set; }

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

		private RemindService()
		{
			this.IsShowSimulatorHint = true;
			this.IsShowSkeletonTrackPoint = true;
		}

		public void Save()
		{
			Option.UserConfig.Save();
		}

		public const string conifgKey = "RemindConfig";

		private static RemindService _Instance;
	}
}
