using System;
using System.Collections.Generic;
using Modules.Communal.MultiLanguage;
using Newtonsoft.Json.Linq;

namespace Modules.Communal.CocoaChina
{
	public class Login
	{
		public Login()
		{
			CocosConsts.LoadCocosUrl();
		}

		public User User { get; private set; }

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

		public event EventHandler<CocoaUserArgs> OnResived;

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
