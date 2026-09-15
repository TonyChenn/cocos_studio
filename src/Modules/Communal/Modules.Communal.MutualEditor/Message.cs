using System;

namespace Modules.Communal.MutualEditor
{
	[Serializable]
	public class Message
	{
		public string SentIP = "127.0.0.1";

		public string Sentport;

		public string ReciveIP = "127.0.0.1";

		public string RecivePort;

		public Action Action;

		public string Data;
	}
}
