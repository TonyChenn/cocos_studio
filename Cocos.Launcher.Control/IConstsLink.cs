using System;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000004 RID: 4
	public interface IConstsLink
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9
		string DomainUrl { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000A RID: 10
		string DocumentUrl { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11
		string Document404 { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12
		string Feedback404 { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000D RID: 13
		string Store404 { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600000E RID: 14
		string StoreUrl { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000F RID: 15
		string RegisterUri { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000010 RID: 16
		string ForgotPasswordUri { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000011 RID: 17
		string LoginUri { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000012 RID: 18
		string AdvertisementUrl { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000013 RID: 19
		string FeedbackUrl { get; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000014 RID: 20
		string ServicePluginInfoUrl { get; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000015 RID: 21
		string UpdateIdentifyXmlUrl { get; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000016 RID: 22
		string RequestCheckUrl { get; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000017 RID: 23
		string UsualQuestionsUrl { get; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000018 RID: 24
		string SearchHotWordsUrl { get; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000019 RID: 25
		string SearchKeywordsUrl { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600001A RID: 26
		string SearchUrl { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600001B RID: 27
		string SystemPlatformByString { get; }
	}
}
