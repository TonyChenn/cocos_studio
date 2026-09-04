using System;

namespace MonoDevelop.Debugger
{
	public class PinnedWatchEventArgs : EventArgs
	{
		private PinnedWatch watch;

		public PinnedWatch Watch => watch;

		public PinnedWatchEventArgs(PinnedWatch watch)
		{
			this.watch = watch;
		}
	}
}
