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
	[DataContract]
	public class UserJsonInfo
	{
		[DataMember]
		public string Uid { get; private set; }

		[DataMember]
		public string Username { get; private set; }

		[DataMember]
		public string ClientVersion { get; private set; }

		[DataMember]
		public string Language { get; private set; }

		[DataMember]
		public string System { get; private set; }

		[DataMember]
		public List<ServicePluginInfo> Tools { get; set; }

		[DataMember]
		public string App_key { get; private set; }

		[DataMember]
		public string Password { get; private set; }

		[DataMember]
		public string Access_token { get; private set; }

		[DataMember]
		public string Sign { get; private set; }

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

		private static UserJsonInfo instance;
	}
}
