using System;

namespace Modules.Communal.AutoUpdate
{
	public class DownloadFinishedArgs : EventArgs
	{
		public bool IsSuccessed { get; private set; }

		public string Output { get; private set; }

		public DownloadFinishedArgs(bool isSuccessed, string output = "")
		{
			this.IsSuccessed = isSuccessed;
			this.Output = output;
		}
	}
}
