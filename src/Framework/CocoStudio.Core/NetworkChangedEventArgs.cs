using System;

namespace CocoStudio.Core
{
	public class NetworkChangedEventArgs : EventArgs
	{
		public bool IsNetworkingSuccessed { get; private set; }

		public NetworkChangedEventArgs(bool isNetworkingSuccessed)
		{
			this.IsNetworkingSuccessed = isNetworkingSuccessed;
		}
	}
}
