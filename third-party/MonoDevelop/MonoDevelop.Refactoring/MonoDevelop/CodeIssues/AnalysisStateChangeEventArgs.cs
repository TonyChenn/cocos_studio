using System;

namespace MonoDevelop.CodeIssues
{
	public class AnalysisStateChangeEventArgs : EventArgs
	{
		public AnalysisState OldState { get; private set; }

		public AnalysisState NewState { get; private set; }

		public AnalysisStateChangeEventArgs(AnalysisState oldState, AnalysisState newState)
		{
			OldState = oldState;
			NewState = newState;
		}
	}
}
