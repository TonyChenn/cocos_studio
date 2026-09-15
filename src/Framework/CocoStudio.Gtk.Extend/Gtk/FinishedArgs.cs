using System;

namespace Gtk
{
	public class FinishedArgs : EventArgs
	{
		public bool IsSuccess { get; private set; }

		public FinishedArgs(bool isSuccess)
		{
			this.IsSuccess = isSuccess;
		}
	}
}
