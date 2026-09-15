using System;
using System.IO;
using System.Xml.Linq;
using CocoStudio.Basic;

namespace Modules.Communal.CocoaChina
{
	public class CocosConsts
	{
		public static string CocosLoginUrl { get; private set; }

		public static string CocosRedirectUrl { get; private set; }

		public static string CocosFirstPageUrl { get; private set; }

		public static string CocosUserInfoUrl { get; private set; }

		public static string CocosAccessTokenUrl { get; private set; }

		public static string CocosLogOutUrl { get; private set; }

		private static string CocosInfoXmlPath
		{
			get
			{
				return Path.Combine(Option.UserCustomerConfigFolder, "CocosLogin4.xml");
			}
		}

		public static void LoadCocosUrl()
		{
			CocosConsts.SetDefaultValue();
		}

		private static void SetDefaultValue()
		{
			CocosConsts.CocosLoginUrl = "https://passport.cocos.com/oauth/token/?";
			CocosConsts.CocosRedirectUrl = "https://passport.cocos.com/oauth/client_signin?";
			CocosConsts.CocosFirstPageUrl = "https://cn.cocos.com";
			CocosConsts.CocosUserInfoUrl = "https://passport.cocos.com/user/info?";
			CocosConsts.CocosAccessTokenUrl = "https://passport.cocos.com/oauth2/access_token?";
			CocosConsts.CocosLogOutUrl = "https://passport.cocos.com/oauth2/logoff?";
		}

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
