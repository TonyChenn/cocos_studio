using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Cocos.Launcher.Control;
using Cocos.Launcher.Library;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000035 RID: 53
	[DataContract]
	public class UserJsonInfo
	{
		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x00008BAE File Offset: 0x00006DAE
		// (set) Token: 0x060001D3 RID: 467 RVA: 0x00008BB6 File Offset: 0x00006DB6
		[DataMember]
		public string Uid { get; private set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x00008BBF File Offset: 0x00006DBF
		// (set) Token: 0x060001D5 RID: 469 RVA: 0x00008BC7 File Offset: 0x00006DC7
		[DataMember]
		public string Username { get; private set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001D6 RID: 470 RVA: 0x00008BD0 File Offset: 0x00006DD0
		// (set) Token: 0x060001D7 RID: 471 RVA: 0x00008BD8 File Offset: 0x00006DD8
		[DataMember]
		public string ClientVersion { get; private set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001D8 RID: 472 RVA: 0x00008BE1 File Offset: 0x00006DE1
		// (set) Token: 0x060001D9 RID: 473 RVA: 0x00008BE9 File Offset: 0x00006DE9
		[DataMember]
		public string Language { get; private set; }

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001DA RID: 474 RVA: 0x00008BF2 File Offset: 0x00006DF2
		// (set) Token: 0x060001DB RID: 475 RVA: 0x00008BFA File Offset: 0x00006DFA
		[DataMember]
		public string System { get; private set; }

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001DC RID: 476 RVA: 0x00008C03 File Offset: 0x00006E03
		// (set) Token: 0x060001DD RID: 477 RVA: 0x00008C0B File Offset: 0x00006E0B
		[DataMember]
		public List<ServicePluginInfo> Tools { get; set; }

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001DE RID: 478 RVA: 0x00008C14 File Offset: 0x00006E14
		// (set) Token: 0x060001DF RID: 479 RVA: 0x00008C1C File Offset: 0x00006E1C
		[DataMember]
		public string App_key { get; private set; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00008C25 File Offset: 0x00006E25
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00008C2D File Offset: 0x00006E2D
		[DataMember]
		public string Password { get; private set; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00008C36 File Offset: 0x00006E36
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00008C3E File Offset: 0x00006E3E
		[DataMember]
		public string Access_token { get; private set; }

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x00008C47 File Offset: 0x00006E47
		// (set) Token: 0x060001E5 RID: 485 RVA: 0x00008C4F File Offset: 0x00006E4F
		[DataMember]
		public string Sign { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x00008C58 File Offset: 0x00006E58
		public static UserJsonInfo Instance
		{
			get
			{
				if (UserJsonInfo.instance == null)
				{
					UserJsonInfo.instance = new UserJsonInfo();
				}
				return UserJsonInfo.instance;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00008C70 File Offset: 0x00006E70
		public UserJsonInfo()
		{
			this.Uid = (this.Username = (this.Password = (this.Access_token = (this.Sign = string.Empty))));
			this.System = ConstantConfig.Constant.SystemPlatformByString;
			this.ClientVersion = Option.EditorVersion.ToString();
			if (LanguageOption.CurrentLanguage == LanguageType.Chinese)
			{
				this.Language = "chinese";
			}
			else if (LanguageOption.CurrentLanguage == LanguageType.Traditional)
			{
				this.Language = "zh-tw";
			}
			else
			{
				this.Language = "english";
			}
			this.App_key = "f2fb1076691c445a46b25e1fcc9e95f2";
			if (Services.LoginService.IsLoginSuccessed)
			{
				this.Uid = Services.LoginService.LoginInfo.UserID;
				this.Username = Services.LoginService.LoginInfo.UserName;
				this.Access_token = Services.LoginService.LoginInfo.Access_token;
				this.Sign = this.GetStoreSign();
			}
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x00008D6C File Offset: 0x00006F6C
		private void Refresh()
		{
			if (Services.LoginService.IsLoginSuccessed)
			{
				this.Uid = Services.LoginService.LoginInfo.UserID;
				this.Username = Services.LoginService.LoginInfo.UserName;
				this.Access_token = Services.LoginService.LoginInfo.Access_token;
				this.Sign = this.GetStoreSign();
				return;
			}
			this.Uid = (this.Username = (this.Access_token = (this.Sign = string.Empty)));
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x00008DF8 File Offset: 0x00006FF8
		private string GetStoreSign()
		{
			string str = string.Format("access_token={0}&app_key={1}&password={2}&uid={3}&username={4}{5}", new object[]
			{
				this.Access_token,
				"f2fb1076691c445a46b25e1fcc9e95f2",
				this.Password,
				this.Uid,
				this.Username,
				"3f579e7429443a3ec0a9062e27766c72"
			});
			return str.EncryptMD5();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008E54 File Offset: 0x00007054
		public string GetPostDataToString()
		{
			this.Refresh();
			List<ServicePluginInfo> list = new List<ServicePluginInfo>();
			if (DownloadService.Instance.AssetManager.AssetModelList != null)
			{
				foreach (Plugin plugin in DownloadService.Instance.AssetManager.AssetModelList.Keys)
				{
					try
					{
						if (plugin.PluginFraction == 100f && File.Exists(plugin.PluginPath) && !string.IsNullOrEmpty(plugin.OpenType))
						{
							ServicePluginInfo item = new ServicePluginInfo(plugin.PluginName, plugin.PluginType, plugin.PluginVersion, plugin.IsInstalled);
							list.Add(item);
						}
					}
					catch (Exception message)
					{
						LogConfig.Output.Error(message);
					}
				}
			}
			this.Tools = list;
			string str = "json=";
			return str + JsonHelper.Stringify(this);
		}

		// Token: 0x040000BA RID: 186
		private static UserJsonInfo instance;
	}
}
