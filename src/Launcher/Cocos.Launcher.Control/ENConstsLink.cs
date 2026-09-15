using System;

namespace Cocos.Launcher.Control
{
	internal class ENConstsLink : ConstsLink
	{
		public override string ForgotPasswordUri
		{
			get
			{
				return "http://cocos2d-x.org/u/forget_password";
			}
		}

		public override string UpdateIdentifyXmlUrl
		{
			get
			{
				return this.DomainUrl + "updateapi/english";
			}
		}

		public override string UsualQuestionsUrl
		{
			get
			{
				return "http://cocostudio.org/help/2.0/faq/english";
			}
		}
	}
}
