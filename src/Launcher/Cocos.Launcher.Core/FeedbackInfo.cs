using System;
using System.Collections.Generic;
using Cocos.Launcher.Library;
using Modules.Communal.MultiLanguage;
using MonoDevelop.Core;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000043 RID: 67
	public class FeedbackInfo
	{
		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000246 RID: 582 RVA: 0x00009B24 File Offset: 0x00007D24
		// (set) Token: 0x06000247 RID: 583 RVA: 0x00009B2C File Offset: 0x00007D2C
		public List<string> QuestionClassifyList { get; private set; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000248 RID: 584 RVA: 0x00009B35 File Offset: 0x00007D35
		// (set) Token: 0x06000249 RID: 585 RVA: 0x00009B3D File Offset: 0x00007D3D
		public int Type_id { get; set; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00009B46 File Offset: 0x00007D46
		public string UID
		{
			get
			{
				return Services.LoginService.LoginInfo.UserID;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600024B RID: 587 RVA: 0x00009B57 File Offset: 0x00007D57
		public string CocosVersion
		{
			get
			{
				return "2.3.3.0";
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600024C RID: 588 RVA: 0x00009B5E File Offset: 0x00007D5E
		public string Email
		{
			get
			{
				return Services.LoginService.LoginInfo.UserName;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600024D RID: 589 RVA: 0x00009B70 File Offset: 0x00007D70
		// (set) Token: 0x0600024E RID: 590 RVA: 0x00009BBC File Offset: 0x00007DBC
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

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600024F RID: 591 RVA: 0x00009BC5 File Offset: 0x00007DC5
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

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000250 RID: 592 RVA: 0x00009BD1 File Offset: 0x00007DD1
		public string OS_version
		{
			get
			{
				return Platform.OSVersion.ToString(2);
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000251 RID: 593 RVA: 0x00009BE0 File Offset: 0x00007DE0
		public string Verify
		{
			get
			{
				string str = this.UID + this.Email;
				return str.EncryptMD5();
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00009C08 File Offset: 0x00007E08
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

		// Token: 0x06000253 RID: 595 RVA: 0x00009C60 File Offset: 0x00007E60
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

		// Token: 0x040000E7 RID: 231
		private string description;
	}
}
