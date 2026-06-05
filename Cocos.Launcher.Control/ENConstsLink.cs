using System;

namespace Cocos.Launcher.Control
{
	// Token: 0x02000008 RID: 8
	internal class ENConstsLink : ConstsLink
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000045 RID: 69 RVA: 0x00002790 File Offset: 0x00000990
		public override string ForgotPasswordUri
		{
			get
			{
				return "http://cocos2d-x.org/u/forget_password";
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000046 RID: 70 RVA: 0x00002797 File Offset: 0x00000997
		public override string UpdateIdentifyXmlUrl
		{
			get
			{
				return this.DomainUrl + "updateapi/english";
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000047 RID: 71 RVA: 0x000027A9 File Offset: 0x000009A9
		public override string UsualQuestionsUrl
		{
			get
			{
				return "http://cocostudio.org/help/2.0/faq/english";
			}
		}
	}
}
