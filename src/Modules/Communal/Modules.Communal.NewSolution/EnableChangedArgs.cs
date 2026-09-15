using System;

namespace Modules.Communal.NewSolution
{
	public class EnableChangedArgs : EventArgs
	{
		public bool IsEnable { get; private set; }

		public EnableChangedArgs(bool isEnable)
		{
			this.IsEnable = isEnable;
		}
	}
}
