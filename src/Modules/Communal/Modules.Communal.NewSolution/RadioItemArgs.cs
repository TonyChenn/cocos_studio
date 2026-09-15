using System;

namespace Modules.Communal.NewSolution
{
	public class RadioItemArgs : EventArgs
	{
		public IRadioItem RadioItem { get; private set; }

		public RadioItemArgs(IRadioItem item)
		{
			this.RadioItem = item;
		}
	}
}
