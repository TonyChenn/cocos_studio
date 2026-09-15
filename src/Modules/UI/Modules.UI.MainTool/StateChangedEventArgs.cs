using System;

namespace Modules.UI.MainTool
{
	public class StateChangedEventArgs : EventArgs
	{
		public string State { get; private set; }

		public StateChangedEventArgs(string state)
		{
			this.State = (state ?? string.Empty);
		}
	}
}
