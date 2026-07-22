using System;
using System.Collections.Generic;
using Modules.Communal.MultiLanguage;
using Newtonsoft.Json.Linq;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x02000007 RID: 7
	public class Login
	{
		// Token: 0x06000022 RID: 34 RVA: 0x00002858 File Offset: 0x00000A58
		public Login()
		{
			CocosConsts.LoadCocosUrl();
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000023 RID: 35 RVA: 0x0000286C File Offset: 0x00000A6C
		// (set) Token: 0x06000024 RID: 36 RVA: 0x00002883 File Offset: 0x00000A83
		public User User { get; private set; }

		// Token: 0x06000025 RID: 37 RVA: 0x0000288C File Offset: 0x00000A8C
		private void ParseCocosMsg(string retStr)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace(retStr))
				{
					if (retStr.Contains("error"))
					{
						this.User.ErrorMsg = Login.GetJsonAnalysis(retStr, "error_description");
					}
					else
					{
						if (retStr.Contains("access_token"))
						{
							this.User.Access_token = Login.GetJsonAnalysis(retStr, "access_token");
						}
						if (retStr.Contains("refresh_token"))
						{
							this.User.Refresh_token = Login.GetJsonAnalysis(retStr, "refresh_token");
						}
						if (retStr.Contains("uid"))
						{
							this.User.UserID = Login.GetJsonAnalysis(retStr, "uid");
						}
					}
				}
			}
			catch (Exception)
			{
				this.User.ErrorMsg = LanguageInfo.NetworkAnomalies;
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002988 File Offset: 0x00000B88
		public static string GetJsonAnalysis(string jsonStream, string fieldName)
		{
			JObject jobject = JObject.Parse(jsonStream);
			JToken value = jobject.GetValue(fieldName);
			string result;
			if (value != null)
			{
				result = value.ToString();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000029C0 File Offset: 0x00000BC0
		public static List<string> GetJsonAnalysis(string jsonStream, string fieldName, string property)
		{
			List<string> list = new List<string>();
			JObject jobject = JObject.Parse(jsonStream);
			JArray jarray = JArray.Parse(jobject[fieldName].ToString());
			if (jarray != null)
			{
				for (int i = 0; i < jarray.Count; i++)
				{
					JObject jobject2 = JObject.Parse(jarray[i].ToString());
					string item = jobject2[property].ToString();
					list.Add(item);
				}
			}
			return list;
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000028 RID: 40 RVA: 0x00002A44 File Offset: 0x00000C44
		// (remove) Token: 0x06000029 RID: 41 RVA: 0x00002A80 File Offset: 0x00000C80
		public event EventHandler<CocoaUserArgs> OnResived;

		// Token: 0x0600002A RID: 42 RVA: 0x00002ABC File Offset: 0x00000CBC
		protected void Resived(User user)
		{
			if (this.OnResived != null)
			{
				this.OnResived(this, new CocoaUserArgs
				{
					User = user
				});
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public void SyncLoginCocos(string name, string password)
		{
			this.User = new User
			{
				UserName = name,
				PassWord = password
			};
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.http_OnResived;
			httpSync.GetSyncResponseOfString(CocosConsts.CocosLoginUrl + this.User.CocosLoginData, "get", "", null);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B6C File Offset: 0x00000D6C
		private void http_OnResived(object sender, HttpSync.HttpSyncArgs e)
		{
			HttpSync httpSync = sender as HttpSync;
			if (httpSync != null)
			{
				httpSync.OnResived -= this.http_OnResived;
			}
			this.ParseCocosMsg(e.Message);
			this.Resived(this.User);
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public void SyncCocosUserInfo(string access_token)
		{
			this.User = new User
			{
				Access_token = access_token
			};
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.http_OnResived;
			httpSync.GetSyncResponseOfString(CocosConsts.CocosUserInfoUrl + this.User.CocosUserInfo, "get", "", null);
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002C24 File Offset: 0x00000E24
		public void SyncCocosUpdateToken(string refresh_token)
		{
			this.User = new User
			{
				Refresh_token = refresh_token
			};
			HttpSync httpSync = new HttpSync();
			httpSync.OnResived += this.http_OnResived;
			httpSync.GetSyncResponseOfString(CocosConsts.CocosAccessTokenUrl + this.User.CocosUpdateToken, "get", "", null);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002C90 File Offset: 0x00000E90
		public void SyncCocosLogOut(string access_token)
		{
			this.User = new User
			{
				Access_token = access_token
			};
			HttpSync httpSync = new HttpSync();
			httpSync.GetSyncResponseOfString(CocosConsts.CocosLogOutUrl + this.User.CocosUserInfo, "get", "", null);
		}
	}
}
