using System;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// A handler that can execute commands of a specific type
	/// </summary>
	// Token: 0x02000029 RID: 41
	public interface IExecutionHandler
	{
		/// <summary>
		/// Determines whether this instance can execute the specified command.
		/// </summary>
		/// <returns>
		/// <c>true</c> if this instance can execute the specified command; otherwise, <c>false</c>.
		/// </returns>
		/// <param name="command">
		/// Command.
		/// </param>
		// Token: 0x06000168 RID: 360
		bool CanExecute(ExecutionCommand command);

		/// <summary>
		/// Executes the specified command
		/// </summary>
		/// <param name="command">
		/// The command
		/// </param>
		/// <param name="console">
		/// Console where to log the output
		/// </param>
		// Token: 0x06000169 RID: 361
		IProcessAsyncOperation Execute(ExecutionCommand command, IConsole console);
	}
}
