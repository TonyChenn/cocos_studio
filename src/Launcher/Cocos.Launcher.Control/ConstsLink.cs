using System;
using CocoStudio.Basic;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Control
{
	public class ConstsLink : IConstsLink
	{
		public string Document404
		{
			get
			{
				return this.Get404ImageID("tutorial");
			}
		}

		public string Feedback404
		{
			get
			{
				return this.Get404ImageID("feedback");
			}
		}

		public string Store404
		{
			get
			{
				return this.Get404ImageID("plugin");
			}
		}

		private string Get404ImageID(string imageName)
		{
			string format = "Cocos.Launcher.Resource.LauncherResource.{0}404_{1}.png";
			string currentName = LanguageAdapter.GetCurrentName(EnumNameFormat.Code, false);
			return string.Format(format, imageName, currentName);
		}

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

		public string DocumentUrl
		{
			get
			{
				string currentName = LanguageAdapter.GetCurrentName(EnumNameFormat.Short, true);
				return string.Format("https://launcher.cocos.com/doc/center_{0}?version={1}", currentName, Option.EditorVersion);
			}
		}

		public virtual string StoreUrl
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return this.domainUrl + string.Format("tools/appstore?version={0}&language={1}&goal=store", Option.EditorVersion, arg);
			}
		}

		public virtual string AdvertisementUrl
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return this.domainUrl + string.Format("tools/advert?language={0}", arg);
			}
		}

		public virtual string RegisterUri
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return string.Format("https://passport.cocos.com/auth/signup?lang={0}", arg);
			}
		}

		public virtual string ForgotPasswordUri
		{
			get
			{
				return "https://passport.cocos.com/auth/forget_pass";
			}
		}

		public virtual string LoginUri
		{
			get
			{
				return "https://passport.cocos.com/sso/signin";
			}
		}

		public string RequestCheckUrl
		{
			get
			{
				return this.domainUrl + "tools/request_test";
			}
		}

		public virtual string FeedbackUrl
		{
			get
			{
				return this.domainUrl + "feedback/add?";
			}
		}

		public virtual string ServicePluginInfoUrl
		{
			get
			{
				return this.domainUrl + "tools/download_list_status";
			}
		}

		public virtual string UpdateIdentifyXmlUrl
		{
			get
			{
				return this.domainUrl + "updateapi";
			}
		}

		public virtual string UsualQuestionsUrl
		{
			get
			{
				return "http://cocostudio.org/help/2.0/faq/chinese";
			}
		}

		public virtual string SearchHotWordsUrl
		{
			get
			{
				return "https://launcher.cocos.com/jsapi/launcher_hot_word";
			}
		}

		public virtual string SearchKeywordsUrl
		{
			get
			{
				return "https://launcher.cocos.com/jsapi/launcher_search_word?";
			}
		}

		public virtual string SearchUrl
		{
			get
			{
				string arg = LanguageAdapter.GetCurrentName(EnumNameFormat.Full, true).ToLower();
				return string.Format("https://launcher.cocos.com/tools/cocos_search?version={0}&language={1}", Option.EditorVersion, arg);
			}
		}

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

		private const string forgotPasswordUri = "https://passport.cocos.com/auth/forget_pass";

		private const string loginUri = "https://passport.cocos.com/sso/signin";

		protected string domainUrl = "http://api.launcher.cocos.com/";
	}
}
