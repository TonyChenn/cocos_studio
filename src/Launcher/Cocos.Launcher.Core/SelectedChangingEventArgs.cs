using System;

namespace Cocos.Launcher.Core
{
	public class SelectedChangingEventArgs : EventArgs
	{
		public bool IsSelected { get; private set; }

		public SelectedChangingEventArgs(bool isSelected)
		{
			this.IsSelected = isSelected;
		}
	}
}
