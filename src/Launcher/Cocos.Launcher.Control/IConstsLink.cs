using System;

namespace Cocos.Launcher.Control
{
	public interface IConstsLink
	{
		string DomainUrl { get; }

		string DocumentUrl { get; }

		string Document404 { get; }

		string Feedback404 { get; }

		string Store404 { get; }

		string StoreUrl { get; }

		string RegisterUri { get; }

		string ForgotPasswordUri { get; }

		string LoginUri { get; }

		string AdvertisementUrl { get; }

		string FeedbackUrl { get; }

		string ServicePluginInfoUrl { get; }

		string UpdateIdentifyXmlUrl { get; }

		string RequestCheckUrl { get; }

		string UsualQuestionsUrl { get; }

		string SearchHotWordsUrl { get; }

		string SearchKeywordsUrl { get; }

		string SearchUrl { get; }

		string SystemPlatformByString { get; }
	}
}
