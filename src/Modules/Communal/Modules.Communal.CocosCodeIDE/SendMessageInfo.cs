using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Modules.Communal.CocosCodeIDE
{
	[DataObject]
	internal class SendMessageInfo
	{
		public string command { get; set; }

		[JsonProperty("params")]
		public string[] Params { get; set; }

		public SendMessageInfo(string command, string[] message)
		{
			this.command = command;
			this.Params = message;
		}

		public SendMessageInfo()
		{
		}
	}
}
