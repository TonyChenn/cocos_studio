using System;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// A user visible mode for executing commands. It can be for example a specific debugger of profiler.
	/// </summary>
	// Token: 0x0200005A RID: 90
	public interface IExecutionMode
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002E4 RID: 740
		string Name { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002E5 RID: 741
		string Id { get; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002E6 RID: 742
		IExecutionHandler ExecutionHandler { get; }
	}
}
