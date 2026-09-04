using System;
using Cocos.Launcher.Library;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x02000008 RID: 8
	public class User
	{
		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00002CEC File Offset: 0x00000EEC
		// (set) Token: 0x06000031 RID: 49 RVA: 0x00002D03 File Offset: 0x00000F03
		public string UserName { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002D0C File Offset: 0x00000F0C
		// (set) Token: 0x06000033 RID: 51 RVA: 0x00002D23 File Offset: 0x00000F23
		public string PassWord { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000034 RID: 52 RVA: 0x00002D2C File Offset: 0x00000F2C
		// (set) Token: 0x06000035 RID: 53 RVA: 0x00002D43 File Offset: 0x00000F43
		public string Access_token { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54 RVA: 0x00002D4C File Offset: 0x00000F4C
		// (set) Token: 0x06000037 RID: 55 RVA: 0x00002D63 File Offset: 0x00000F63
		public string Email { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000038 RID: 56 RVA: 0x00002D6C File Offset: 0x00000F6C
		// (set) Token: 0x06000039 RID: 57 RVA: 0x00002D84 File Offset: 0x00000F84
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

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003A RID: 58 RVA: 0x00002D90 File Offset: 0x00000F90
		// (set) Token: 0x0600003B RID: 59 RVA: 0x00002DA7 File Offset: 0x00000FA7
		public string UserID { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00002DB0 File Offset: 0x00000FB0
		// (set) Token: 0x0600003D RID: 61 RVA: 0x00002DC7 File Offset: 0x00000FC7
		public string Refresh_token { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600003F RID: 63 RVA: 0x00002DDC File Offset: 0x00000FDC
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

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000040 RID: 64 RVA: 0x00002E30 File Offset: 0x00001030
		public string CocosLoginSign
		{
			get
			{
				string text = "app_key=f2fb1076691c445a46b25e1fcc9e95f2&grant_type=password&password=" + this.PassWord + "&username=" + this.UserName;
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000041 RID: 65 RVA: 0x00002E70 File Offset: 0x00001070
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

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002EB8 File Offset: 0x000010B8
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

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000043 RID: 67 RVA: 0x00002F10 File Offset: 0x00001110
		public string CocosUserInfoSign
		{
			get
			{
				string text = "access_token=" + this.Access_token + "&app_key=f2fb1076691c445a46b25e1fcc9e95f2";
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000044 RID: 68 RVA: 0x00002F4C File Offset: 0x0000114C
		public string CocosUpdateTokenSign
		{
			get
			{
				string text = "refresh_token=" + this.Refresh_token + "&grant_type=refresh_token";
				text += "3f579e7429443a3ec0a9062e27766c72";
				return text.EncryptMD5();
			}
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002F88 File Offset: 0x00001188
		public string CocosUserInfo
		{
			get
			{
				return "access_token=" + this.Access_token + "&app_key=f2fb1076691c445a46b25e1fcc9e95f2&sign=" + this.CocosUserInfoSign;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002FB8 File Offset: 0x000011B8
		public string CocosUpdateToken
		{
			get
			{
				return "refresh_token=" + this.Refresh_token + "&grant_type=refresh_token&sign=" + this.CocosUpdateTokenSign;
			}
		}

		// Token: 0x04000019 RID: 25
		private string errorMsg;
	}
}
