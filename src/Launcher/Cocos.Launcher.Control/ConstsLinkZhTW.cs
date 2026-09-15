using System;
using CocoStudio.Basic;

namespace Cocos.Launcher.Control
{
	internal class ConstsLinkZhTW : ConstsLink
	{
		public override string UpdateIdentifyXmlUrl
		{
			get
			{
				return this.DomainUrl + "updateapi/zh-tw";
			}
		}

		public override string StoreUrl
		{
			get
			{
				return this.domainUrl + string.Format("tools/appstore?version={0}&language=zh-tw&goal=store", Option.EditorVersion);
			}
		}

		public override string AdvertisementUrl
		{
			get
			{
				return this.domainUrl + string.Format("tools/advert?language=chinese", new object[0]);
			}
		}

		public override string SearchUrl
		{
			get
			{
				return string.Format("https://launcher.cocos.com/tools/cocos_search?version={0}&language=zh-tw", Option.EditorVersion);
			}
		}

		public override string RegisterUri
		{
			get
			{
				return "https://passport.cocos.com/auth/signup?lang=chinese";
			}
		}
	}
}
