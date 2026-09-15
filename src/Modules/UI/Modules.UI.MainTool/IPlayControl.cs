using System;

namespace Modules.UI.MainTool
{
	public interface IPlayControl
	{
		event EventHandler<StateChangedEventArgs> StateChanged;

		bool CanPlay { get; }

		bool CanStop { get; }

		bool Play();

		void Stop();
	}
}
