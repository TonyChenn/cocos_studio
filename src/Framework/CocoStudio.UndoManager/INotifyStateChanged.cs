using System;
using System.ComponentModel;

namespace CocoStudio.UndoManager
{
	public interface INotifyStateChanged : INotifyPropertyChanged
	{
		event EventHandler<StateChangedEventArgs> StateChanged;
	}
}
