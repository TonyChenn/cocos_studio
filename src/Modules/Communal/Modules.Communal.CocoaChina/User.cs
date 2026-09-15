using System;
using Cocos.Launcher.Library;

namespace Modules.Communal.CocoaChina
{
	public class User
	{
		public string UserName { get; set; }

		public string PassWord { get; set; }

		public string Access_token { get; set; }

		public string Email { get; set; }

		public string ErrorMsg
		{
			get
			{
				return this.errorMsg;
			}
			set
			{
				this.errorMsg = value;
			}
		}

		public string UserID { get; set; }

		public string Refresh_token { get; set; }

		public string CocosLoginData
		{
			get
			{
				return string.Concat(new string[]
				{
					"app_key=f2fb1076691c445a46b25e1fcc9e95f2&grant_type=password&password=",
					this.PassWord,
					"&username=",
					this.UserName,
					"&sign=",
					this.CocosLoginSign
				});
			}
		}

		public string CocosLoginSign
		{
			get
			{
				string text = "app_key=f2fb1076691c445a46b25e1fcc9e95f2&grant_type=password&password=" + this.PassWord + "&username=" + this.UserName;
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		public string CocosRedirectSign
		{
			get
			{
				string cocoaUrlEncode = CocosConsts.CocosFirstPageUrl.GetCocoaUrlEncode();
				string text = "access_token=" + this.Access_token + "&app_key=f2fb1076691c445a46b25e1fcc9e95f2&url=" + cocoaUrlEncode;
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		public string CocosRedirectData
		{
			get
			{
				string cocoaUrlEncode = CocosConsts.CocosFirstPageUrl.GetCocoaUrlEncode();
				return string.Concat(new string[]
				{
					"access_token=",
					this.Access_token,
					"&sign=",
					this.CocosRedirectSign,
					"&app_key=f2fb1076691c445a46b25e1fcc9e95f2&url=",
					cocoaUrlEncode
				});
			}
		}

		public string CocosUserInfoSign
		{
			get
			{
				string text = "access_token=" + this.Access_token + "&app_key=f2fb1076691c445a46b25e1fcc9e95f2";
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		public string CocosUpdateTokenSign
		{
			get
			{
				string text = "refresh_token=" + this.Refresh_token + "&grant_type=refresh_token";
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		public string CocosUserInfo
		{
			get
			{
				return "access_token=" + this.Access_token + "&app_key=f2fb1076691c445a46b25e1fcc9e95f2&sign=" + this.CocosUserInfoSign;
			}
		}

		public string CocosUpdateToken
		{
			get
			{
				return "refresh_token=" + this.Refresh_token + "&grant_type=refresh_token&sign=" + this.CocosUpdateTokenSign;
			}
		}

		private string errorMsg;
	}
}
