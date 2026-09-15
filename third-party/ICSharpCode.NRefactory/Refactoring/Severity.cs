using System;

namespace ICSharpCode.NRefactory.Refactoring
{
	/// <summary>
	/// The severity influences how the task bar reacts on found issues.
	/// </summary>
	public enum Severity
	{
		/// <summary>
		/// None means that the task bar doesn't show the issue.
		/// </summary>
		None,
		/// <summary>
		/// Errors are shown in red and that the task bar is in error state if 1 error is found.
		/// </summary>
		Error,
		/// <summary>
		/// Warnings are shown in yellow and set the task bar to warning state (if no error is found).
		/// </summary>
		Warning,
		/// <summary>
		/// Suggestions are shown in green and doesn't influence the task bar state
		/// </summary>
		Suggestion,
		/// <summary>
		/// Hints are shown in blue and doesn't influence the task bar state
		/// </summary>
		Hint
	}
}
