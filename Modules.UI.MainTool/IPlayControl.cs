using System;

namespace Modules.UI.MainTool
{
	// Token: 0x0200000E RID: 14
	public interface IPlayControl
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000051 RID: 81
		// (remove) Token: 0x06000052 RID: 82
		event EventHandler<StateChangedEventArgs> StateChanged;

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000053 RID: 83
		bool CanPlay { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000054 RID: 84
		bool CanStop { get; }

		// Token: 0x06000055 RID: 85
		bool Play();

		// Token: 0x06000056 RID: 86
		void Stop();
	}
}
