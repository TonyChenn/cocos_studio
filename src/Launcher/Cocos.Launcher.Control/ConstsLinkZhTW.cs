using System;
using CocoStudio.Basic;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000006 RID: 6
	internal class ConstsLinkZhTW : ConstsLink
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00002580 File Offset: 0x00000780
		public override string UpdateIdentifyXmlUrl
		{
			get
			{
				return this.DomainUrl + "updateapi/zh-tw";
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000033 RID: 51 RVA: 0x00002592 File Offset: 0x00000792
		public override string StoreUrl
		{
			get
			{
				return this.domainUrl + string.Format("tools/appstore?version={0}&language=zh-tw&goal=store", Option.EditorVersion);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000034 RID: 52 RVA: 0x000025AE File Offset: 0x000007AE
		public override string AdvertisementUrl
		{
			get
			{
				return this.domainUrl + string.Format("tools/advert?language=chinese", new object[0]);
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000035 RID: 53 RVA: 0x000025CC File Offset: 0x000007CC
		public override string SearchUrl
		{
			get
			{
				return string.Format("https://launcher.cocos.com/tools/cocos_search?version={0}&language=zh-tw", Option.EditorVersion);
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000036 RID: 54 RVA: 0x000025EA File Offset: 0x000007EA
		public override string RegisterUri
		{
			get
			{
				return "https://passport.cocos.com/auth/signup?lang=chinese";
			}
		}
	}
}
