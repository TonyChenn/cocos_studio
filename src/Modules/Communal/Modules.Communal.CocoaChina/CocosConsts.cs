using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;

namespace Modules.Communal.CocoaChina
{
	// Token: 0x02000003 RID: 3
	public class CocosConsts
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000021B4 File Offset: 0x000003B4
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000021CA File Offset: 0x000003CA
		public static string CocosLoginUrl { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000021D4 File Offset: 0x000003D4
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000021EA File Offset: 0x000003EA
		public static string CocosRedirectUrl { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021F4 File Offset: 0x000003F4
		// (set) Token: 0x06000009 RID: 9 RVA: 0x0000220A File Offset: 0x0000040A
		public static string CocosFirstPageUrl { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002214 File Offset: 0x00000414
		// (set) Token: 0x0600000B RID: 11 RVA: 0x0000222A File Offset: 0x0000042A
		public static string CocosUserInfoUrl { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000C RID: 12 RVA: 0x00002234 File Offset: 0x00000434
		// (set) Token: 0x0600000D RID: 13 RVA: 0x0000224A File Offset: 0x0000044A
		public static string CocosAccessTokenUrl { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002254 File Offset: 0x00000454
		// (set) Token: 0x0600000F RID: 15 RVA: 0x0000226A File Offset: 0x0000046A
		public static string CocosLogOutUrl { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000010 RID: 16 RVA: 0x00002274 File Offset: 0x00000474
		private static string CocosInfoXmlPath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "CocosLogin4.xml");
			}
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002295 File Offset: 0x00000495
		public static void LoadCocosUrl()
		{
			CocosConsts.SetDefaultValue();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000022A0 File Offset: 0x000004A0
		private static void SetDefaultValue()
		{
			CocosConsts.CocosLoginUrl = "https://passport.cocos.com/oauth/token/?";
			CocosConsts.CocosRedirectUrl = "https://passport.cocos.com/oauth/client_signin?";
			CocosConsts.CocosFirstPageUrl = "https://cn.cocos.com";
			CocosConsts.CocosUserInfoUrl = "https://passport.cocos.com/user/info?";
			CocosConsts.CocosAccessTokenUrl = "https://passport.cocos.com/oauth2/access_token?";
			CocosConsts.CocosLogOutUrl = "https://passport.cocos.com/oauth2/logoff?";
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000022F0 File Offset: 0x000004F0
		private static void Save()
		{
			try
			{
				XElement xelement = new XElement("CocosLogin", new object[]
				{
					new XElement("CocosLoginUrl", CocosConsts.CocosLoginUrl),
					new XElement("CocosRedirectUrl", CocosConsts.CocosRedirectUrl),
					new XElement("CocosFirstPageUrl", CocosConsts.CocosFirstPageUrl),
					new XElement("CocosUserInfoUrl", CocosConsts.CocosUserInfoUrl)
				});
				xelement.Save(CocosConsts.CocosInfoXmlPath);
			}
			catch (Exception message)
			{
				LogConfig.Logger.Error(message);
			}
		}
	}
}
