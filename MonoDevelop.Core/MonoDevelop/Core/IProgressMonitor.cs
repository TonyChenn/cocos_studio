using System;
using System.IO;

namespace MonoDevelop.Core
{
	// Token: 0x0200001C RID: 28
	public interface IProgressMonitor : IDisposable
	{
		// Token: 0x060000D3 RID: 211
		void BeginTask(string name, int totalWork);

		// Token: 0x060000D4 RID: 212
		void BeginStepTask(string name, int totalWork, int stepSize);

		// Token: 0x060000D5 RID: 213
		void EndTask();

		// Token: 0x060000D6 RID: 214
		void Step(int work);

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D7 RID: 215
		TextWriter Log { get; }

		// Token: 0x060000D8 RID: 216
		void ReportWarning(string message);

		// Token: 0x060000D9 RID: 217
		void ReportSuccess(string message);

		// Token: 0x060000DA RID: 218
		void ReportError(string message, Exception exception);

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000DB RID: 219
		bool IsCancelRequested { get; }

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060000DC RID: 220
		// (remove) Token: 0x060000DD RID: 221
		event MonitorHandler CancelRequested;

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000DE RID: 222
		IAsyncOperation AsyncOperation { get; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000DF RID: 223
		object SyncRoot { get; }
	}
}
