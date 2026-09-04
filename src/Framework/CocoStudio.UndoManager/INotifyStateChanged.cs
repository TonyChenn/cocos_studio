using System;
using System.ComponentModel;

namespace CocoStudio.UndoManager
{
	// Token: 0x02000016 RID: 22
	public interface INotifyStateChanged : INotifyPropertyChanged
	{
		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000A9 RID: 169
		// (remove) Token: 0x060000AA RID: 170
		event EventHandler<StateChangedEventArgs> StateChanged;
	}
}
