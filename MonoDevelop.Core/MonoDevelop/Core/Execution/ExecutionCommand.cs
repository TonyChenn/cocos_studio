using System;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// Set of parameters to be used to execute a file or project
	/// </summary>
	/// <remarks>
	/// This is the base class for types of commands that can be used
	/// to run a project or file. This class only contains the data
	/// required to run the project, but not the actual execution logic.
	/// The execution logic is provided by classes that implement
	/// IExecutionHandler. A project generates an ExecutionCommand
	/// instance, and a user can select a IExecutionHandler to
	/// run it.
	/// </remarks>
	// Token: 0x020000AE RID: 174
	public abstract class ExecutionCommand
	{
		/// <summary>
		/// Execution target. For example, a specific device.
		/// </summary>
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060005F7 RID: 1527 RVA: 0x00016715 File Offset: 0x00014915
		// (set) Token: 0x060005F8 RID: 1528 RVA: 0x0001671D File Offset: 0x0001491D
		public ExecutionTarget Target { get; set; }
	}
}
