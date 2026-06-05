using System;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// An execution handler that executes on a particular execution target.
	/// </summary>
	// Token: 0x0200002E RID: 46
	public interface ITargetedExecutionHandler : IExecutionHandler
	{
		/// <summary>
		/// The execution target
		/// </summary>
		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600017A RID: 378
		ExecutionTarget Target { get; }
	}
}
