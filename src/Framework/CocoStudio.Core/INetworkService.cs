using System;

namespace CocoStudio.Core
{
	public interface INetworkService
	{
		bool IsOK { get; }

		void Intinalize(string requestUrl, int? interval = null);

		event EventHandler<NetworkChangedEventArgs> NetworkChanged;

		void TryRequest();
	}
}
