using System;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000005 RID: 5
	public class ConstsLink : IConstsLink
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600001C RID: 28 RVA: 0x0000239D File Offset: 0x0000059D
		public string Document404
		{
			get
			{
				return this.Get404ImageID("tutorial");
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000023AA File Offset: 0x000005AA
		public string Feedback404
		{
			get
			{
				return this.Get404ImageID("feedback");
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023B7 File Offset: 0x000005B7
		public string Store404
		{
			get
			{
				return this.Get404ImageID("plugin");
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000023C4 File Offset: 0x000005C4
		private string Get404ImageID(string imageName)
		{
			string format = "Cocos.Launcher.Resource.LauncherResource.{0}404_{1}.png";
			string currentName = LanguageAdapter.GetCurrentName(EnumNameFormat.Code, false);
			return string.Format(format, imageName, currentName);
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000021 RID: 33 RVA: 0x000023F8 File Offset: 0x000005F8
		// (set) Token: 0x06000020 RID: 32 RVA: 0x000023E7 File Offset: 0x000005E7
		public virtual string DomainUrl
		{
			get
			{
				return this.domainUrl;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					this.domainUrl = value;
				}
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000022 RID: 34 RVA: 0x00002400 File Offset: 0x00000600
		public string DocumentUrl
		{
			get
			{
				string currentName = LanguageAdapter.GetCurrentName(EnumNameFormat.Short, true);
				return string.Format("https://launcher.cocos.com/doc/center_{0}?version={1}", currentName, Option.EditorVersion);
			}
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002428 File Offset: 0x00000628
		public virtual string StoreUrl
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return this.domainUrl + string.Format("tools/appstore?version={0}&language={1}&goal=store", Option.EditorVersion, arg);
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000024 RID: 36 RVA: 0x00002460 File Offset: 0x00000660
		public virtual string AdvertisementUrl
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return this.domainUrl + string.Format("tools/advert?language={0}", arg);
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000025 RID: 37 RVA: 0x00002490 File Offset: 0x00000690
		public virtual string RegisterUri
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return string.Format("https://passport.cocos.com/auth/signup?lang={0}", arg);
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000026 RID: 38 RVA: 0x000024B5 File Offset: 0x000006B5
		public virtual string ForgotPasswordUri
		{
			get
			{
				return "https://passport.cocos.com/auth/forget_pass";
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000027 RID: 39 RVA: 0x000024BC File Offset: 0x000006BC
		public virtual string LoginUri
		{
			get
			{
				return "https://passport.cocos.com/sso/signin";
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000028 RID: 40 RVA: 0x000024C3 File Offset: 0x000006C3
		public string RequestCheckUrl
		{
			get
			{
				return this.domainUrl + "tools/request_test";
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000024D5 File Offset: 0x000006D5
		public virtual string FeedbackUrl
		{
			get
			{
				return this.domainUrl + "feedback/add?";
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600002A RID: 42 RVA: 0x000024E7 File Offset: 0x000006E7
		public virtual string ServicePluginInfoUrl
		{
			get
			{
				return this.domainUrl + "tools/download_list_status";
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600002B RID: 43 RVA: 0x000024F9 File Offset: 0x000006F9
		public virtual string UpdateIdentifyXmlUrl
		{
			get
			{
				return this.domainUrl + "updateapi";
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000250B File Offset: 0x0000070B
		public virtual string UsualQuestionsUrl
		{
			get
			{
				return "http://cocostudio.org/help/2.0/faq/chinese";
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002512 File Offset: 0x00000712
		public virtual string SearchHotWordsUrl
		{
			get
			{
				return "https://launcher.cocos.com/jsapi/launcher_hot_word";
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00002519 File Offset: 0x00000719
		public virtual string SearchKeywordsUrl
		{
			get
			{
				return "https://launcher.cocos.com/jsapi/launcher_search_word?";
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00002520 File Offset: 0x00000720
		public virtual string SearchUrl
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return string.Format("https://launcher.cocos.com/tools/cocos_search?version={0}&language={1}", Option.EditorVersion, arg);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000254C File Offset: 0x0000074C
		public virtual string SystemPlatformByString
		{
			get
			{
				if (Platform.IsMac)
				{
					return "mac";
				}
				if (Environment.Is64BitOperatingSystem)
				{
					return "win64";
				}
				return "win32";
			}
		}

		// Token: 0x04000022 RID: 34
		private const string forgotPasswordUri = "https://passport.cocos.com/auth/forget_pass";

		// Token: 0x04000023 RID: 35
		private const string loginUri = "https://passport.cocos.com/sso/signin";

		// Token: 0x04000024 RID: 36
		protected string domainUrl = "http://api.launcher.cocos.com/";
	}
}
