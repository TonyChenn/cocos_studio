using System;

namespace MonoDevelop.Core.Execution
{
	/// <summary>
	/// Used to create commands from file paths
	/// </summary>
	// Token: 0x02000248 RID: 584
	public interface ICommandFactory
	{
		/// <summary>
		/// Creates a command, or returns null if a command can't be created from the given path
		/// </summary>
		/// <returns>The command, or null if a command can't be created from the given path</returns>
		/// <param name="path">Path.</param>
		// Token: 0x06001590 RID: 5520
		ProcessExecutionCommand CreateCommand(string path);
	}
}
