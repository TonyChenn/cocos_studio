using System;

namespace Modules.Communal.AutoUpdate
{
	public class ProgressChangedArgs : EventArgs
	{
		public float Progress { get; private set; }

		public ProgressChangedArgs(float progress)
		{
			this.Progress = progress;
		}
	}
}
