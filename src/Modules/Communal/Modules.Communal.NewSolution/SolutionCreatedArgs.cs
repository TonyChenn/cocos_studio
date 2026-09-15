using System;

namespace Modules.Communal.NewSolution
{
	public class SolutionCreatedArgs : EventArgs
	{
		public string DefaultScenePath { get; private set; }

		public SolutionCreatedArgs(string defaultScenePath)
		{
			this.DefaultScenePath = defaultScenePath;
		}
	}
}
