using System;

namespace Modules.Communal.Render.Model
{
	public class CurrentToolChangedEventArgs : EventArgs
	{
		public ITool Current { get; private set; }

		public CurrentToolChangedEventArgs(ITool currentTool)
		{
			this.Current = currentTool;
		}
	}
}
