using System;

namespace ICSharpCode.NRefactory.Refactoring
{
	/// <summary>
	/// The severity influences how the task bar reacts on found issues.
	/// </summary>
	// Token: 0x0200013B RID: 315
	public enum Severity
	{
		/// <summary>
		/// None means that the task bar doesn't show the issue.
		/// </summary>
		// Token: 0x040003AE RID: 942
		None,
		/// <summary>
		/// Errors are shown in red and that the task bar is in error state if 1 error is found.
		/// </summary>
		// Token: 0x040003AF RID: 943
		Error,
		/// <summary>
		/// Warnings are shown in yellow and set the task bar to warning state (if no error is found).
		/// </summary>
		// Token: 0x040003B0 RID: 944
		Warning,
		/// <summary>
		/// Suggestions are shown in green and doesn't influence the task bar state
		/// </summary>
		// Token: 0x040003B1 RID: 945
		Suggestion,
		/// <summary>
		/// Hints are shown in blue and doesn't influence the task bar state
		/// </summary>
		// Token: 0x040003B2 RID: 946
		Hint
	}
}
