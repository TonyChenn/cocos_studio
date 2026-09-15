using System;
using System.Net;

namespace CocoStudio.UserStatistics
{
	internal class RequestState
	{
		public HttpWebRequest Request { get; private set; }

		public RequestState(HttpWebRequest request)
		{
			this.Request = request;
		}
	}
}
