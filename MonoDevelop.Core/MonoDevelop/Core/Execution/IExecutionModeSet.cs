using System;
using System.Collections.Generic;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// A collection of exectution modes
	/// </summary>
	/// <remarks>
	/// For example, the mode set "Debug" provides several execution modes
	/// for all the supported debuggers
	/// </remarks>
	// Token: 0x020000AA RID: 170
	public interface IExecutionModeSet
	{
		/// <summary>
		/// Name of the execution mode set
		/// </summary>
		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060005E7 RID: 1511
		string Name { get; }

		/// <summary>
		/// Execution modes provided by this set
		/// </summary>
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060005E8 RID: 1512
		IEnumerable<IExecutionMode> ExecutionModes { get; }
	}
}
