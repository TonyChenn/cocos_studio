using System;
using System.Collections.Generic;
using Cocos.Launcher.Library;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	public class FeedbackInfo
	{
		public List<string> QuestionClassifyList { get; private set; }

		public int Type_id { get; set; }

		public string UID
		{
			get
			{
				return Services.LoginService.LoginInfo.UserID;
			}
		}

		public string CocosVersion
		{
			get
			{
				return "2.3.3.0";
			}
		}

		public string Email
		{
			get
			{
				return Services.LoginService.LoginInfo.UserName;
			}
		}

		public string Description
		{
			get
			{
				this.description = this.description.Replace("&", "").Replace("=", "").Replace("#", "");
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		public int OS_id
		{
			get
			{
				if (!Platform.IsWindows)
				{
					return 1;
				}
				return 2;
			}
		}

		public string OS_version
		{
			get
			{
				return Platform.OSVersion.ToString(2);
			}
		}

		public string Verify
		{
			get
			{
				string str = this.UID + this.Email;
				return str.EncryptMD5();
			}
		}

		public FeedbackInfo()
		{
			this.QuestionClassifyList = new List<string>
			{
				LanguageInfo.Launcher_Feature,
				LanguageInfo.Launcher_CodeIssues,
				LanguageInfo.Launcher_BugFeedback,
				LanguageInfo.Launcher_UserFeedback,
				LanguageInfo.Launcher_Other
			};
		}

		internal string GetFeedbackInfo()
		{
			return string.Concat(new object[]
			{
				"uid=",
				this.UID,
				"&type_id=",
				this.Type_id,
				"&email=",
				this.Email,
				"&description=",
				this.Description,
				"&ide=",
				this.CocosVersion,
				"&os_id=",
				this.OS_id,
				"&os_version=",
				this.OS_version,
				"&verify=",
				this.Verify
			});
		}

		private string description;
	}
}
